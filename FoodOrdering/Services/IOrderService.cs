namespace FoodOrdering.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetUserOrders();
        Task<IEnumerable<Order>> GetOrders();

        Task ChangeOrderStatus(UpdateOrderStatusModel data);
        Task<List<OrderStatus>> GetOrderStatuses();
        Order? GetOrderById(int id);
        public IEnumerable<OrderDetails> GetOrderDetails(int orderId);

    }
}
