using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Fast_Food1.Data;
using Fast_Food1.Extensions;
//using Fast_Food1.Interfaces;
using Fast_Food1.Models;
using Newtonsoft.Json;
using Fast_Food1.Service;
namespace MyBookApp.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly IFoodService _foodService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public CheckOutController(IFoodService foodService, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _foodService = foodService;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("cart");
            var paymentMethods = await _context.PaymentMethods.ToListAsync();
            ViewBag.PaymentMethods = paymentMethods;
            if (cart != null)
            {
                ViewBag.total = cart.Sum(s => (decimal)(s.Food!.Price * (100 - s.Food!.DiscountToday) / 100) * s.Quantity);
            }
            else
            {
                cart = new List<CartItem>();

                ViewBag.total = 0;
            }
            ViewBag.Cart = cart;
            DataOrder data = new DataOrder();
            return View(data);
        }

        public IActionResult PaymentCompleted() 
        {
            return View();
        }
        public IActionResult Failed()
        {
            return View();
        }

        [HttpPost, ActionName("CreateOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(string address, decimal total, int MethodId)
        {
            // Lấy ID của phương thức thanh toán được chọn
            var selectedPaymentMethod = await _context.PaymentMethods.FirstOrDefaultAsync(m => m.Id == MethodId);
            if (selectedPaymentMethod == null)
            {
                // Xử lý khi không tìm thấy phương thức thanh toán
                return RedirectToAction(nameof(Failed));
            }

            Order order = new Order
            {
                PaymentMethodsId = selectedPaymentMethod.Id,
                Address = address,
                TotalPrice = total,
                UserId = _userManager.GetUserId(User),
                Status = false,
                OrderDate = DateTime.Now,
                ReceivedDate = DateTime.Now,
            };
            if (ModelState.IsValid)
            {
                _context.Order.Add(order);
                await _context.SaveChangesAsync();

                var cart = HttpContext.Session.Get<List<CartItem>>("cart");
                Order detail = new Order();
                foreach (var item in _context.Order!)
                {
                    detail = item;
                }
                if (detail == null)
                {
                    return NotFound();
                }
                foreach (var item in cart!)
                {
                    var updatebook = _context.Food.FirstOrDefault(x => x.Id == item.Food!.Id);
                    updatebook!.Amount = updatebook!.Amount - item.Quantity;
                    updatebook!.Sold = updatebook!.Sold + item.Quantity;
                    _context.Food.Update(updatebook);
                    var orderDetail = new OrderDetail()
                    {
                        OrderId = detail.Id,
                        FoodId = item.Food!.Id,
                        Quantity = item.Quantity,
                        Subtotal = item.SubTotal
                    };
                    _context.OrderDetail.Add(orderDetail);
                    await _context.SaveChangesAsync();
                }
                HttpContext.Session.Remove("cart");
                HttpContext.Session.Clear();
                return RedirectToAction(nameof(PaymentCompleted));
            }
            return RedirectToAction(nameof(Failed));
        }
       


    }
}
