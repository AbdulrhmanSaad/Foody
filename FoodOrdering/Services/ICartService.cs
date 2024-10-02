using Stripe.Checkout;

namespace FoodOrdering.Services
{
    public interface ICartService
    {
        public Task<int> AddItem(int productId, int quantity);
        public Task<ShoppingCart> GetUserCart();
        public Task<int> RemoveItem(int productId);
        public Task<int> GetCartItemsCount(string userId = "");
        public Task<Session?> CheckOut(PaymentDto payment);
        public bool CheckOutCOD(PaymentDto payment);


    }
}
