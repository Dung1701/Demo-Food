using Fast_Food1.Data;
using Fast_Food1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Fast_Food1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index(string FoodCategory, string searchString)
        {
            if (_context.Food == null)
            {
                return Problem("Entity set is null.");
            }

            // Lấy danh sách sản phẩm hot (ví dụ: sản phẩm có đánh giá cao)
            var hotFoods = await _context.Food.Where(f => f.Rating >= 4).Take(5).ToListAsync();

            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Food
                                            orderby m.Category
                                            select m.Category;
            var foods = from m in _context.Food
                        select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                foods = foods.Where(s => s.Category!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(FoodCategory))
            {
                foods = foods.Where(x => x.Category == FoodCategory);
            }

            var listFoodCategory = new FoodCategoryView
            {
                Category = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Foods = await foods.ToListAsync(),
                HotFoods = hotFoods
            };

            return View(listFoodCategory);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
