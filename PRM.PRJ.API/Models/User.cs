using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Data;

namespace PRM.PRJ.API.Models
{
    public class User : IdentityUser
    {
        public String LastName { get; set; }
        public String Phone { get; set; }
        public String Address { get; set; }
        public String Birthday { get; set; }

        public ICollection<Order> Orders { get; set; }
        public virtual List<CartItem>? CartItem { get; set; }
    }
}
