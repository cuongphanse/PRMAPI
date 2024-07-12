using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRM.PRJ.API.Models.ViewModel;
using PRM.PRJ.API.Services;

namespace PRM.PRJ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-payment")]
        public IActionResult CreatePayment(float amount,string orderid, string locale)
        {
            string paymentUrl = _paymentService.CreatePaymentUrl(amount, orderid, locale);
            return Ok(new { PaymentUrl = paymentUrl });
        }

        [HttpGet("response")]
        public async Task<IActionResult> PaymentResponse([FromQuery] VnPayResponse response)
        {
            bool isValid = _paymentService.ValidatePaymentResponse(response);
            if (isValid)
            {
                await _paymentService.SaveTransactionAsync(response);
                // Process the response and update the order status in your system
                return Ok(new { Message ="Payment successful" });
            }
            else
            {
                return BadRequest(new { Message = "Invalid payment response" });
            }
        }
    }
}
