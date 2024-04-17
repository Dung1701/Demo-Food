using System.ComponentModel.DataAnnotations.Schema;

namespace Fast_Food1.Models
{
    [Serializable]
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
        public int FoodId { get; set; }
        [ForeignKey("FoodId")]
        public Food? Food { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        public decimal Subtotal { get; set; } // Tổng tiền của chi tiết đơn hàng
        public bool HasRated { get; set; } = false;
    }
}
