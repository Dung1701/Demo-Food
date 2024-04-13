using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fast_Food1.Models;
using Microsoft.AspNetCore.Identity;

namespace Fast_Food1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Fast_Food1.Models.Food> Food { get; set; } = default!;
        public DbSet<Fast_Food1.Models.CartItem> CartItem { get; set; } = default!;
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Fast_Food1.Models.PaymentMethods> PaymentMethods { get; set; } = default!;
        public DbSet<Fast_Food1.Models.Order> Order { get; set; } = default!;
        public DbSet<Fast_Food1.Models.OrderDetail> OrderDetail { get; set; } = default!;
        public DbSet<Fast_Food1.Models.OrderHistory> OrderHistory { get; set; } = default!;
    }
}
