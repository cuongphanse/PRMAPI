namespace PRM.PRJ.API.Models.ViewModel
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public IFormFile? urlImage { get; set; }
    }
}
