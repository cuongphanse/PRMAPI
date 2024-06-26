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

    public class UserSignIn
    {
        public String UserName { get; set; }
        public String Password { get; set; }
    }
}
