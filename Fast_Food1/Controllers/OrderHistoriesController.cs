using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fast_Food1.Data;
using Fast_Food1.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Fast_Food1.Controllers
{
    public class OrderHistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderHistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Kiểm tra người dùng đã đăng nhập hay chưa
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Login", "Account"); // Chuyển hướng đến trang đăng nhập
            //}

            // Lấy ID của người dùng đang đăng nhập
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Lọc ra các đơn hàng của người dùng hiện tại
            var orders = await _context.Order
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Food)
                .Include(o => o.PaymentMethods)
                .Where(o => o.UserId == userId) // Giả sử có một trường UserId trong bảng Order để lưu ID của người dùng
                .ToListAsync();

            // Tính toán tổng giá cho mỗi đơn hàng
            foreach (var order in orders)
            {
                order.TotalPrice = order.OrderDetails.Sum(od => od.Quantity * od.Food.Price);
            }

            return View(orders);
        }


        // GET: OrderHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Food)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        public async Task<IActionResult> OrderConfirm(int Id)
        {
            var order = _context.Order.FirstOrDefault(x => x.Id == Id);
            if (order != null)
            {
                order!.ReceivedDate = DateTime.Now;
                order.Status = true;
                _context.Order.Update(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
