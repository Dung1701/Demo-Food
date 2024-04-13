using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fast_Food1.Data;
using Fast_Food1.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Fast_Food1.Service;
using System.Security.Claims;

namespace Fast_Food1.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class CartItemsController : Controller
    {
        private readonly IFoodService _foodService;
        private readonly ApplicationDbContext _context;

        public CartItemsController(IFoodService foodService, ApplicationDbContext context)
        {
            _foodService = foodService;
            _context = context;
        }

        public IActionResult Index()
        {
            var serializedCart = HttpContext.Session.GetString("cart");
            var cart = serializedCart != null ? JsonConvert.DeserializeObject<List<CartItem>>(serializedCart) : new List<CartItem>();

            if (cart != null)
            {
                ViewBag.total = cart.Sum(s => s.Quantity * s.Food?.Price);
            }
            else
            {
                cart = new List<CartItem>();
                ViewBag.total = 0;
            }

            var paymentMethods = _foodService.GetPaymentMethods();
            ViewBag.PaymentMethods = paymentMethods;

            return View(cart);
        }

        public IActionResult Buy(int id)
        {
            var food = _foodService.GetFood(id);
            var serializedCart = HttpContext.Session.GetString("cart");
            var cart = serializedCart != null ? JsonConvert.DeserializeObject<List<CartItem>>(serializedCart) : new List<CartItem>();

            if (cart == null)
            {
                cart = new List<CartItem>();
                cart.Add(new CartItem { Food = food, Quantity = 1 });
            }
            else
            {
                int index = cart.FindIndex(w => w.Food?.Id == id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { Food = food, Quantity = 1 });
                }
            }

            serializedCart = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("cart", serializedCart);
            return RedirectToAction("Index");
        }

        public IActionResult Add(int id)
        {
            var food = _foodService.GetFood(id);
            var serializedCart = HttpContext.Session.GetString("cart");
            var cart = serializedCart != null ? JsonConvert.DeserializeObject<List<CartItem>>(serializedCart) : new List<CartItem>();

            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            else
            {
                var cartItem = cart.FirstOrDefault(item => item.FoodId == id);
                if (cartItem != null)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { FoodId = id, Quantity = 1 });
                }
            }

            serializedCart = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("cart", serializedCart);
            return RedirectToAction("Index");
        }

        public IActionResult Minus(int id)
        {
            var food = _foodService.GetFood(id);

            var serializedCart = HttpContext.Session.GetString("cart");
            var cart = serializedCart != null ? JsonConvert.DeserializeObject<List<CartItem>>(serializedCart) : new List<CartItem>();

            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            else
            {
                var cartItem = cart.FirstOrDefault(item => item.FoodId == id);
                if (cartItem != null)
                {
                    cartItem.Quantity--;

                    if (cartItem.Quantity == 0)
                    {
                        int indexToRemove = -1;
                        for (int i = 0; i < cart.Count; i++)
                        {
                            if (cart[i].FoodId == id)
                            {
                                indexToRemove = i;
                                break;
                            }
                        }
                        if (indexToRemove != -1)
                        {
                            cart.RemoveAt(indexToRemove);
                        }
                    }
                }
            }
            serializedCart = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("cart", serializedCart);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var serializedCart = HttpContext.Session.GetString("cart");
            var cart = serializedCart != null ? JsonConvert.DeserializeObject<List<CartItem>>(serializedCart) : new List<CartItem>();

            if (cart != null)
            {
                var cartItem = cart.FirstOrDefault(item => item.FoodId == id);
                if (cartItem != null)
                {
                    cart.Remove(cartItem);
                }
                serializedCart = JsonConvert.SerializeObject(cart);
                HttpContext.Session.SetString("cart", serializedCart);
            }

            return RedirectToAction("Index");
        }
        public int GetCartCount()
        {
            var serializedCart = HttpContext.Session.GetString("cart");
            var cart = serializedCart != null ? JsonConvert.DeserializeObject<List<CartItem>>(serializedCart) : new List<CartItem>();
            return cart.Count;
        }


        private bool CartItemExists(int id)
        {
            return _context.CartItem.Any(e => e.Id == id);
        }
    }
}
