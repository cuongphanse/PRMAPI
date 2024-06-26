using System.ComponentModel.DataAnnotations;

namespace PRM.PRJ.API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public String urlImage { get; set; }
    }
}
