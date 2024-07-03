using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRM.PRJ.API.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int Quantity { get; set; }
        public virtual Product? Products { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
