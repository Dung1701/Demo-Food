using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fast_Food1.Data;
using Fast_Food1.Models;
using Newtonsoft.Json;
using Fast_Food1.Service;

namespace Fast_Food1.Controllers
{
    public class PaymentMethodsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFoodService _foodService;
        public PaymentMethodsController(ApplicationDbContext context, IFoodService foodService)
        {
            _context = context;
            _foodService = foodService;
        }

        // GET: PaymentMethods
        public async Task<IActionResult> Index()
        {
            return View(await _context.PaymentMethods.ToListAsync());
        }

        // GET: PaymentMethods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentMethods = await _context.PaymentMethods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paymentMethods == null)
            {
                return NotFound();
            }

            return View(paymentMethods);
        }

        // GET: PaymentMethods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PaymentMethods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,MethodImage")] PaymentMethods paymentMethods)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paymentMethods);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paymentMethods);
        }

        // GET: PaymentMethods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentMethods = await _context.PaymentMethods.FindAsync(id);
            if (paymentMethods == null)
            {
                return NotFound();
            }
            return View(paymentMethods);
        }

        // POST: PaymentMethods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,MethodImage")] PaymentMethods paymentMethods)
        {
            if (id != paymentMethods.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymentMethods);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentMethodsExists(paymentMethods.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(paymentMethods);
        }

        // GET: PaymentMethods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentMethods = await _context.PaymentMethods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paymentMethods == null)
            {
                return NotFound();
            }

            return View(paymentMethods);
        }

        // POST: PaymentMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paymentMethods = await _context.PaymentMethods.FindAsync(id);
            if (paymentMethods != null)
            {
                _context.PaymentMethods.Remove(paymentMethods);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult OrderDetails()
        {
            var orderDetailsJson = TempData["OrderDetailsJson"] as string;
            var orderDetails = JsonConvert.DeserializeObject<List<OrderDetail>>(orderDetailsJson);

            // Lấy danh sách phương thức thanh toán từ IFoodService
            var paymentMethods = _foodService.GetPaymentMethods();

            // Truyền danh sách orderDetails và paymentMethods sang view
            ViewBag.OrderDetails = orderDetails;
            ViewBag.PaymentMethods = paymentMethods;

            return View();
        }

        private bool PaymentMethodsExists(int id)
        {
            return _context.PaymentMethods.Any(e => e.Id == id);
        }
    }
}
