using Microsoft.AspNetCore.Identity;
using Fast_Food1.Models;
namespace Fast_Food1.Data
{
    public class ApplicationUser: IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Comment>? comments { get; set; }


    }
}
