using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PRM.PRJ.API.Models

{
    public class Transaction
    {
        public int Id { get; set; }
        public string Amount { get; set; }
        public string BankCode { get; set; }
        public string BankTranNo { get; set; }
        public string CardType { get; set; }
        public string? OrderInfo { get; set; }
        public string PayDate { get; set; }
        public string ResponseCode { get; set; }
        public string TmnCode { get; set; }
        public string TransactionNo { get; set; }
        public string TransactionStatus { get; set; }
        public string TxnRef { get; set; }
        public string SecureHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}
