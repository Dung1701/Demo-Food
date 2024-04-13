using System.ComponentModel.DataAnnotations.Schema;

namespace Fast_Food1.Models
{
    public class DataOrder
    {
        public int Id { get; set; }
        public string? Address { get; set; }
        public int MethodId { get; set; }
        public decimal Total { get; set; }
    }
}
