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
using Microsoft.AspNetCore.Identity;

namespace Fast_Food1.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class FoodsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public FoodsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        // GET: Foods/Details/5
        //public async Task<IActionResult> Details(int id)
        //{
        //    var food = await _context.Food
        //        .Include(f => f.Comments) // Bao gồm các comment
        //        .FirstOrDefaultAsync(f => f.Id == id);

        //    if (food == null)
        //    {
        //        return NotFound();
        //    }

        //    // Kiểm tra xem người dùng đã mua hàng hay chưa
        //    var userId = _userManager.GetUserId(User);
        //    var completedOrders = await _context.Order
        //        .Where(o => o.UserId == userId && o.Status == true) // Đơn hàng đã hoàn thành của người dùng hiện tại
        //        .ToListAsync();

        //    ViewBag.HasCompletedOrder = completedOrders.Any();

        //    // Lấy danh sách các sản phẩm thuộc cùng danh mục với sản phẩm hiện tại
        //    var suggestedFoods = await _context.Food.Where(m => m.Category == food.Category && m.Id != id).ToListAsync();

        //    ViewData["SuggestedFoods"] = suggestedFoods;

        //    // Truyền danh sách người dùng vào ViewBag
        //    ViewBag.Users = await _context.Users.ToListAsync();

        //    return View(food);
        //}
        public async Task<IActionResult> Details(int id)
        {
            var food = await _context.Food
                .Include(f => f.Comments)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (food == null)
            {
                return NotFound();
            }

            // Kiểm tra xem người dùng đã mua hàng hay chưa
            var userId = _userManager.GetUserId(User);
            var completedOrders = await _context.Order
                .Where(o => o.UserId == userId && o.Status == true) // Đơn hàng đã hoàn thành của người dùng hiện tại
                .ToListAsync();

            ViewBag.HasCompletedOrder = completedOrders.Any(); // Kiểm tra xem người dùng đã có đơn hàng hoàn thành hay không

            // Lấy danh sách các sản phẩm thuộc cùng danh mục với sản phẩm hiện tại
            var suggestedFoods = await _context.Food
                .Where(m => m.Category == food.Category && m.Id != id)
                .ToListAsync();

            ViewData["SuggestedFoods"] = suggestedFoods;

            // Truyền danh sách người dùng vào ViewBag
            ViewBag.Users = await _context.Users.ToListAsync();

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

        // GET: Moviehis/Edit/5
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

        // POST: Moviehis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Price,Category,Rating,Image,Amount,Sold,DiscountToday,FinalPrice,RatingAverage")] Food food)
        {
            if (id != food.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(food);
                    await _context.SaveChangesAsync();
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
        //[HttpPost, ActionName("AddComment")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddComment(int Id, string comment, int rating)
        //{
        //    var food = _context.Food.Include(f => f.Comments).FirstOrDefault(m => m.Id == Id);
        //    if (food == null)
        //    {
        //        return NotFound();
        //    }
        //    if (string.IsNullOrWhiteSpace(comment))
        //    {
        //        // Trả về một lỗi hoặc thông báo cho người dùng biết rằng họ cần nhập một bình luận hợp lệ
        //        ModelState.AddModelError("comment", "Please enter a valid comment.");
        //        return RedirectToAction(nameof(Details), new { id = Id });
        //    }
        //    var newComment = new Comment
        //    {
        //        FoodId = Id,
        //        Content = comment,
        //        CreatedDate = DateTime.Now,
        //        UserId = _userManager.GetUserId(User),
        //        Rating = rating // Lấy giá trị rating từ form
        //    };

        //    _context.Add(newComment);
        //    await _context.SaveChangesAsync();

        //    // Tính toán trung bình đánh giá
        //    var comments = food.Comments.ToList();
        //    int numberOfRatings = comments.Count;
        //    decimal totalRating = comments.Sum(c => c.Rating);
        //    food.Rating = (int)Math.Round(totalRating / numberOfRatings); // Cập nhật rating của món ăn

        //    // Cập nhật trung bình đánh giá vào món ăn
        //    _context.Update(food);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(Details), new { id = Id });
        //}
        [HttpPost, ActionName("AddComment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int Id, string comment, int rating)
        {
            var food = _context.Food.Include(f => f.Comments).FirstOrDefault(m => m.Id == Id);
            if (food == null)
            {
                return NotFound();
            }

            // Xác định người dùng đã mua hàng và đánh giá sản phẩm
            var userId = _userManager.GetUserId(User);
            var completedOrders = await _context.Order
                 .Include(o => o.OrderDetails) // Bổ sung để lấy danh sách chi tiết đơn hàng
                .Where(o => o.UserId == userId && o.Status == true) // Đơn hàng đã hoàn thành của người dùng hiện tại
                   .ToListAsync();

            var hasCompletedOrder = completedOrders.Any(); // Kiểm tra xem người dùng đã có đơn hàng hoàn thành hay không

            if (!hasCompletedOrder)
            {
                // Trả về một lỗi hoặc thông báo cho người dùng biết rằng họ cần mua hàng trước khi đánh giá
                ModelState.AddModelError("comment", "You need to complete an order before adding a comment.");
                return RedirectToAction(nameof(Details), new { id = Id });
            }

            // Thêm đánh giá vào cơ sở dữ liệu
            var newComment = new Comment
            {
                FoodId = Id,
                Content = comment,
                CreatedDate = DateTime.Now,
                UserId = userId,
                Rating = rating
            };

            _context.Add(newComment);
            await _context.SaveChangesAsync();

            // Cập nhật trạng thái HasRated của từng chi tiết đơn hàng
            foreach (var order in completedOrders)
            {
                foreach (var detail in order.OrderDetails)
                {
                    if (detail.FoodId == Id)
                    {
                        detail.HasRated = true;
                        _context.Update(detail);
                    }
                }
            }
            await _context.SaveChangesAsync();

            // Cập nhật trạng thái HasRated của đơn hàng thành true
            var orderToUpdate = completedOrders.FirstOrDefault();
            if (orderToUpdate != null)
            {
                orderToUpdate.HasRated = true;
                _context.Update(orderToUpdate);
                await _context.SaveChangesAsync();
            }

            // Tính toán trung bình đánh giá và cập nhật lại rating của sản phẩm
            var comments = food.Comments.ToList();
            int numberOfRatings = comments.Count;
            decimal totalRating = comments.Sum(c => c.Rating);
            food.Rating = (int)Math.Round(totalRating / numberOfRatings); // Cập nhật rating của món ăn

            // Cập nhật trung bình đánh giá vào món ăn
            _context.Update(food);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = Id });
        }
    }
}
