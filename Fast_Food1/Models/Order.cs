using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Fast_Food1.Data;

namespace Fast_Food1.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public int PaymentMethodsId { get; set; }
        [ForeignKey("PaymentMethodsId")]
        public PaymentMethods? PaymentMethods { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        public decimal TotalPrice { get; set; }
        public string? Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime ReceivedDate { get; set; }
        public bool Status { get; set; } = false;
        public bool HasRated { get; set; } = false;
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}

