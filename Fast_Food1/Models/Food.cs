using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fast_Food1.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }
        public string? Category { get; set; }
        public int? Rating { get; set; }
        public string? Image { get; set; }
        public ICollection<Comment> Comments { get; set; }

        [Display(Name = "Stock")]
        public int Amount { get; set; }
        public int Sold { get; set; }

        [Display(Name = "Discount")]
        public int DiscountToday { get; set; }

        [Column(TypeName = "decimal(18, 0)")] // Loại cột được xác định
        public decimal FinalPrice { get; set; }
    }
}
