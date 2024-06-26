using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRM.PRJ.API.Data;

namespace PRM.PRJ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Product);
        }
    }
}
