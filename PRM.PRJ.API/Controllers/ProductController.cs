using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM.PRJ.API.Data;
using PRM.PRJ.API.Models;
using PRM.PRJ.API.Models.ViewModel;
using PRM.PRJ.API.Service;

namespace PRM.PRJ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ProductController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Product);
        }

        [HttpPost]

        public async Task<IActionResult> CreateProduct([FromForm]ProductDTO product)
        {
            string url = await FirebaseService.UploadImage(product.urlImage);
            var newProduct = new Product();
            newProduct.urlImage = url;
            newProduct.Name = product.Name;
            newProduct.Description = product.Description;
            newProduct.Price = product.Price;
            newProduct.StockQuantity = product.StockQuantity;

            _context.Product.Add(newProduct);
            var result = _context.SaveChanges();
            if(result == 0) {
                return BadRequest();
            }
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDTO productDto)
        {
            var existingProduct = await _context.Product.FindAsync(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            // Update properties of existing product
            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.Price = productDto.Price;
            existingProduct.StockQuantity = productDto.StockQuantity;

            // Check if a new image is provided
            if (productDto.urlImage != null)
            {
                string imageUrl = await FirebaseService.UploadImage(productDto.urlImage);
                existingProduct.urlImage = imageUrl;
            }

            _context.Product.Update(existingProduct);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return Ok(existingProduct); // Return updated product
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existingProduct = await _context.Product.FindAsync(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            _context.Product.Remove(existingProduct);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return Ok(); // Successfully deleted
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _context.Product.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        

    }
}
