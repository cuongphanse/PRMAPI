using System.ComponentModel.DataAnnotations;

namespace PRM.PRJ.API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        // Các thuộc tính khác cho đơn hàng

        // Khóa ngoại
        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
