using System.ComponentModel.DataAnnotations;

namespace PRM.PRJ.API.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        // Các thuộc tính khác cho chi tiết đơn hàng

        // Khóa ngoại
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
