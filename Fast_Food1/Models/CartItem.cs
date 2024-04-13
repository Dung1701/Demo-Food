using System.ComponentModel.DataAnnotations.Schema;

namespace Fast_Food1.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int FoodId { get; set; } // Thêm trường FoodId để lưu trữ Id của Food
        public Food? Food { get; set; } // Thêm thuộc tính Food để lưu trữ đối tượng Food
        public int Quantity { get; set; }

        private decimal _SubTotal;
        public decimal SubTotal
        {
            get { return _SubTotal; }
            set { _SubTotal = CalculateSubTotal(); }
        }

        private decimal CalculateSubTotal()
        {
            if (Food != null)
            {
                return (decimal)(Food.Price * Quantity);
            }
            else
            {
                return 0; // hoặc giá trị mặc định khác tùy theo yêu cầu của bạn
            }
        }
    }
}
