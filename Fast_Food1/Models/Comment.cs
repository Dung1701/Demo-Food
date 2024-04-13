using System.ComponentModel.DataAnnotations.Schema;

namespace Fast_Food1.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        // Foreign Key relationship with Movie model
        public int FoodId { get; set; }

        [ForeignKey("FoodId")]
        public Food Food { get; set; }
    }
}
