using Fast_Food1.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fast_Food1.Models
{
    public class OrderHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int PaymentMethodsId { get; set; }
        public string PaymentMethodName { get; set; }
        public decimal TotalPrice { get; set; }
        public string Address { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public bool Status { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("PaymentMethodId")]
        public PaymentMethods PaymentMethods { get; set; }
    }
}
