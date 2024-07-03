using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM.PRJ.API.Data;
using PRM.PRJ.API.Models;

namespace PRM.PRJ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public CartController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.CartItem);
        }

        [HttpPost("addToCart")]
        public async Task<IActionResult> AddToCart(string userId, int productId, int quantity)
        {
            // Retrieve current user (if authenticated)
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Find the cart items for the current user
            var cartItem = await _context.CartItem
                .FirstOrDefaultAsync(c => c.User.Id == userId && c.ProductId == productId);

            if (cartItem == null)
            {
                // If the product is not in the cart, create a new cart item
                cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    CreateAt = DateTime.UtcNow,
                    User = user // Assign current user to cart item
                };

                _context.CartItem.Add(cartItem);
            }
            else
            {
                // If the product is already in the cart, update the quantity
                cartItem.Quantity += quantity;
                cartItem.UpdateAt = DateTime.UtcNow;

                _context.CartItem.Update(cartItem);
            }

            var result = _context.SaveChanges();

            if (result == 0)
            {
                return BadRequest();
            }

            return Ok();
        }
        [HttpPut("updateQuantity")]
        public async Task<IActionResult> UpdateQuantity(string userId, int productId, int quantity)
        {
            // Retrieve current user (if authenticated)
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Find the cart item for the current user and product
            var cartItem = await _context.CartItem
                .FirstOrDefaultAsync(c => c.User.Id == userId && c.ProductId == productId);

            if (cartItem == null)
            {
                return NotFound("Product not found in the cart.");
            }

            // Update the quantity
            cartItem.Quantity = quantity;
            cartItem.UpdateAt = DateTime.UtcNow;

            _context.CartItem.Update(cartItem);
            var result = _context.SaveChanges();

            if (result > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to update quantity.");
            }
        }
        [HttpDelete("deleteFromCart")]
        public async Task<IActionResult> DeleteFromCart(string userId, int productId)
        {
            // Retrieve current user (if authenticated)
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Find the cart item for the current user and product
            var cartItem = await _context.CartItem
                .FirstOrDefaultAsync(c => c.User.Id == userId && c.ProductId == productId);

            if (cartItem == null)
            {
                return NotFound("Product not found in the cart.");
            }

            // Remove the cart item from the context
            _context.CartItem.Remove(cartItem);
            var result = _context.SaveChanges();

            if (result > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to delete product from cart.");
            }
        }

    }
}
