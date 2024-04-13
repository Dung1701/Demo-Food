using Microsoft.AspNetCore.Identity;
using Fast_Food1.Models;
namespace Fast_Food1.Data
{
    public class ApplicationUser: IdentityUser
    {
        public ICollection<Order>? Orders { get; set; }
    }
}
