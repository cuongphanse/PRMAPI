using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PRM.PRJ.API.Data;
using PRM.PRJ.API.Models;
using PRM.PRJ.API.Models.ViewModel;
using System;
using System.Threading.Tasks;

namespace PRM.PRJ.API.Services
{
    public interface IPaymentService
    {
        string CreatePaymentUrl(float amount, string orderDescription, string locale);
        bool ValidatePaymentResponse(VnPayResponse response);
        Task SaveTransactionAsync(VnPayResponse response);
    }
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        public PaymentService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
         
            _context = context;
        }

        public string CreatePaymentUrl(float amount, string orderDescription, string locale)
        {
            var vnPay = new VnPayLibrary();

            vnPay.AddRequestData("vnp_Version", _configuration["VnPay:Version"]);
            vnPay.AddRequestData("vnp_Command", _configuration["VnPay:Command"]);
            vnPay.AddRequestData("vnp_TmnCode", _configuration["VnPay:TmnCode"]);
            vnPay.AddRequestData("vnp_Amount", (amount * 100).ToString());
            vnPay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnPay.AddRequestData("vnp_CurrCode", _configuration["VnPay:CurrCode"]);
            vnPay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(_httpContextAccessor.HttpContext));
            vnPay.AddRequestData("vnp_Locale", string.IsNullOrEmpty(locale) ? _configuration["VnPay:Locale"] : locale);
            vnPay.AddRequestData("vnp_OrderInfo", orderDescription);
            vnPay.AddRequestData("vnp_OrderType", "other");
            vnPay.AddRequestData("vnp_ReturnUrl", _configuration["VnPay:ReturnUrl"]);
            vnPay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());
            
            string paymentUrl = vnPay.CreateRequestUrl(_configuration["VnPay:BaseUrl"], _configuration["VnPay:HashSecret"]);

            return paymentUrl;
        }

        public bool ValidatePaymentResponse(VnPayResponse response)
        {
            var vnPay = new VnPayLibrary();

            vnPay.AddResponseData("vnp_Amount", response.vnp_Amount);
            vnPay.AddResponseData("vnp_BankCode", response.vnp_BankCode);
            vnPay.AddResponseData("vnp_BankTranNo", response.vnp_BankTranNo);
            vnPay.AddResponseData("vnp_CardType", response.vnp_CardType);
            vnPay.AddResponseData("vnp_OrderInfo", response.vnp_OrderInfo);
            vnPay.AddResponseData("vnp_PayDate", response.vnp_PayDate);
            vnPay.AddResponseData("vnp_ResponseCode", response.vnp_ResponseCode);
            vnPay.AddResponseData("vnp_TmnCode", response.vnp_TmnCode);
            vnPay.AddResponseData("vnp_TransactionNo", response.vnp_TransactionNo);
            vnPay.AddResponseData("vnp_TransactionStatus", response.vnp_TransactionStatus);
            vnPay.AddResponseData("vnp_TxnRef", response.vnp_TxnRef);

            string hashSecret = _configuration["VnPay:HashSecret"];
            return vnPay.ValidateSignature(response.vnp_SecureHash, hashSecret);
        }

        public async Task SaveTransactionAsync(VnPayResponse response)
        {
            var transaction = new Transaction
            {
                Amount = response.vnp_Amount,
                BankCode = response.vnp_BankCode,
                BankTranNo = response.vnp_BankTranNo,
                CardType = response.vnp_CardType,
                OrderId = int.Parse(response.vnp_OrderInfo),
                PayDate = response.vnp_PayDate,
                ResponseCode = response.vnp_ResponseCode,
                TmnCode = response.vnp_TmnCode,
                TransactionNo = response.vnp_TransactionNo,
                TransactionStatus = response.vnp_TransactionStatus,
                TxnRef = response.vnp_TxnRef,
                SecureHash = response.vnp_SecureHash,
                CreatedAt = DateTime.UtcNow,
                OrderInfo = "Null"
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
