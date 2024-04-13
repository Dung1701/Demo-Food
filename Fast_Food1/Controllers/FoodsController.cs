using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fast_Food1.Data;
using Fast_Food1.Models;
using Microsoft.AspNetCore.Authorization;

namespace Fast_Food1.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class FoodsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string FoodCategory, string searchString)
        {

            if (_context.Food == null)
            {
                return Problem("Entity set is null.");
            }

            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Food
                                            orderby m.Category
                                            select m.Category;
            var foods = from m in _context.Food
                        select m;
            foreach (var food in foods)
            {
                // Tính toán giá cuối cùng dựa trên giá và giảm giá
                food.FinalPrice = food.Price - (food.Price * food.DiscountToday / 100);
            }

            // Lọc và sắp xếp danh sách sản phẩm theo đánh giá và giảm giá
            var highRatedDiscountedFoods = foods.Where(f => f.Rating >= 4 && f.DiscountToday > 20)
                                                .OrderByDescending(f => f.Rating)
                                                .ThenByDescending(f => f.DiscountToday);
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
                Foods = await foods.ToListAsync()
            };

            return View(listFoodCategory);
        }

        // GET: Foods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food.FirstOrDefaultAsync(m => m.Id == id);
            if (food == null)
            {
                return NotFound();
            }

            // Lấy danh sách các sản phẩm thuộc cùng danh mục với sản phẩm hiện tại
            var suggestedFoods = await _context.Food.Where(m => m.Category == food.Category && m.Id != id).ToListAsync();

            ViewData["SuggestedFoods"] = suggestedFoods;

            return View(food);
        }


        // GET: Foods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Price,Category,Rating,Image,Amount,Sold,DiscountToday")] Food food)
        {
            if (ModelState.IsValid)
            {
                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(food);
        }

        // GET: Foods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Price,Category,Rating,Image,Amount,Sold,DiscountToday")] Food food)
        {
            if (id != food.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(food);
                    //await _context.SaveChangesAsync();();
                    _context.Update(food);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodExists(food.Id))
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
            return View(food);
        }


        // GET: Foods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food
                .FirstOrDefaultAsync(m => m.Id == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var food = await _context.Food.FindAsync(id);
            if (food != null)
            {
                _context.Food.Remove(food);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodExists(int id)
        {
            return _context.Food.Any(e => e.Id == id);
        }
        // POST: Foods/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int foodId, string comment)
        {
            var food = await _context.Food.FirstOrDefaultAsync(m => m.Id == foodId);

            if (food == null)
            {
                return NotFound();
            }

            var newComment = new Comment
            {

                FoodId = foodId,
                Content = comment,
                CreatedDate = DateTime.Now
            };

            _context.Add(newComment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = foodId });
        }
    }
}
