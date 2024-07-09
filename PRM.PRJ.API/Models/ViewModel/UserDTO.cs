using System.ComponentModel.DataAnnotations;

namespace PRM.PRJ.API.Models.ViewModel
{
    public class UserDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public String Birthday { get; set; }
        [StringLength(10)]
        public String? Phone { get; set; }
        public bool IsAdmin { get; set; }
    }
    public class UserVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public String FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Birthday { get; set; }
    }
    public class UserUpdateDTO
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public String Birthday { get; set; }
        public String? Phone { get; set; }
    }
    public class UserSignIn
    {
        public String UserName { get; set; }
        public String Password { get; set; }
    }
}
