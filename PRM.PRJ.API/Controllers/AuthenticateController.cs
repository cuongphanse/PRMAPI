using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRM.PRJ.API.Constants;
using PRM.PRJ.API.Models;
using PRM.PRJ.API.Models.ViewModel;
using System.Threading.Tasks;

namespace PRM.PRJ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticateController(UserManager<User> userManager, SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp(UserDTO signUpModel)
        {
            var emailExist = await _userManager.FindByEmailAsync(signUpModel.Email);
            var userNameExist = await _userManager.FindByNameAsync(signUpModel.UserName);
            if (emailExist != null)
            {
                return BadRequest("Email đã tồn tại trên hệ thống!");
            }
            else if (userNameExist != null)
            {
                return BadRequest("Username này đã tồn tại trên hệ thống!");
            }
            var user = _mapper.Map<User>(signUpModel);

            var result = await _userManager.CreateAsync(user, signUpModel.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(AppRole.Customer))
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppRole.Customer));
                }

                if (!await _roleManager.RoleExistsAsync(AppRole.Admin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppRole.Admin));
                }

                if (signUpModel.IsAdmin)
                {
                    await _userManager.AddToRoleAsync(user, AppRole.Admin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, AppRole.Customer);
                }
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn(UserSignIn model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    return NotFound("Sai tên đăng nhập hoặc mật khẩu");
                }
                else
                {
                    return Ok(user.Id);
                }
            }
            else
            {
                return NotFound("Sai tên đăng nhập hoặc mật khẩu");
            }

        }
        //[Authorize]
        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            var userDTOs = users.Select(user => new UserVM
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                Address = user.Address,
                Birthday = user.Birthday
            }).ToList();

            return Ok(userDTOs);
        }

        //[Authorize]
        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> UpdateUser(UserUpdateDTO updateUserModel, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Update user properties from updateUserModel
            
            user.Email = updateUserModel.Email;
            user.FirstName = updateUserModel.FirstName;
            user.LastName = updateUserModel.LastName;
            user.PhoneNumber = updateUserModel.Phone;
            user.Address = updateUserModel.Address;
            user.Birthday = updateUserModel.Birthday;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        //[Authorize]
        [HttpDelete("deleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
