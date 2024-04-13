using Fast_Food1.Models;

namespace Fast_Food1.Service
{
    public interface IFoodService
    {
        IEnumerable<CartItem> GetCartItems();
        Food GetFood(int id);
        IEnumerable<Food> GetFoods();
        IEnumerable<PaymentMethods> GetPaymentMethods();
        PaymentMethods GetPaymentMethod(int PaymentMethodsId);
    }
}
