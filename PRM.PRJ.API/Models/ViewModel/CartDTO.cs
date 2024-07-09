namespace PRM.PRJ.API.Models.ViewModel
{
    public class CartDTO
    {
        public int Id {  get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public String urlImage { get; set; }
    }
}
