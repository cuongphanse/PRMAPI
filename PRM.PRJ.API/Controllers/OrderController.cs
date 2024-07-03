using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRM.PRJ.API.Data;
using PRM.PRJ.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRM.PRJ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(string userId)
        {
            try
            {
                // Retrieve current user
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                // Get user's cart items
                var cartItems = await _context.CartItem
                    .Include(c => c.Products)
                    .Where(c => c.User.Id == user.Id)
                    .ToListAsync();

                if (cartItems == null || cartItems.Count == 0)
                {
                    return BadRequest("Cart is empty.");
                }

                // Calculate total amount for the order
                decimal totalAmount = cartItems.Sum(c => c.Quantity * c.Products.Price);

                // Create a new order
                var order = new Order
                {
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = totalAmount,
                    UserId = user.Id,
                    OrderDetails = new List<OrderDetail>()
                };

                foreach (var cartItem in cartItems)
                {
                    // Create order detail for each cart item
                    var orderDetail = new OrderDetail
                    {
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Products.Price,
                        ProductId = cartItem.ProductId,
                        Product = cartItem.Products
                    };

                    order.OrderDetails.Add(orderDetail);
                }

                // Save order to database
                _context.Order.Add(order);
                await _context.SaveChangesAsync();

                // Clear cart items after checkout
                _context.CartItem.RemoveRange(cartItems);
                await _context.SaveChangesAsync();

                return Ok(order.Id); // Return the newly created order ID
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
