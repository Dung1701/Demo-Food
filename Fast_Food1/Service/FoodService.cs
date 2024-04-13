using Fast_Food1.Data;
using Fast_Food1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Fast_Food1.Service
{
    public class FoodService : IFoodService
    {
        private readonly ApplicationDbContext _context;

        public FoodService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Food GetFood(int id)
        {
            return _context.Food.FirstOrDefault(w => w.Id == id);
        }

        public IEnumerable<Food> GetFoods()
        {
            return _context.Food.ToList();
        }

        public IEnumerable<CartItem> GetCartItems()
        {
            return _context.CartItem.ToList();
        }

        public IEnumerable<PaymentMethods> GetPaymentMethods()
        {
            return _context.PaymentMethods.ToList();
        }

        public PaymentMethods GetPaymentMethod(int paymentMethodId)
        {
            return _context.PaymentMethods.FirstOrDefault(p => p.Id == paymentMethodId);
        }
    }
}
