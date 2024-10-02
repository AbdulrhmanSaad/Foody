namespace FoodOrdering.Services
{
    public class OrderService: IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public OrderService(ApplicationDbContext context,IHttpContextAccessor httpContextAccessor,UserManager<IdentityUser>userManager)
        {
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task ChangeOrderStatus(UpdateOrderStatusModel data)
        {
            var order=await _context.Orders.FindAsync(data.OrderId);
            if(order==null)
                throw new Exception($"Order with id: {data.OrderId} doesn't found");

            order.OrderStatusId = data.orderStatusId;
            await _context.SaveChangesAsync();
        }

        public Order? GetOrderById(int id)
        {
            var order = _context.Orders.Find(id);
            return order;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            var orders=await _context.Orders
                                     .OrderBy(o=>o.CreatedDate)
                                     .Include(o=>o.OrderStatus)
                                     .Include(O=>O.Paymant)
                                     .Include(o=>o.OrderDetails)
                                     .ThenInclude(o=>o.Product)
                                     .ThenInclude(o=>o.Category)
                                    .ToListAsync();
            return orders;
        }
        public  IEnumerable<OrderDetails> GetOrderDetails(int orderId)
        {
            var order = _context.OrderDetails.Include(o=>o.Product).Where(o=>o.OrderId==orderId).ToList();
            return order;
        }

        public Task<List<OrderStatus>> GetOrderStatuses()
        {
            var orderStatus=_context.OrderStatuses.ToListAsync();
            return orderStatus;
        }

        public async Task<IEnumerable<Order>> GetUserOrders()
        {
            var userId = GetUser();
            if(string.IsNullOrEmpty(userId)) 
                throw new Exception("User NOT logged in");
            var order =await _context.Orders
                .OrderByDescending(o => o.CreatedDate)
                
                .Include(o=>o.OrderStatus)
                .Include(o => o.OrderDetails)
                .ThenInclude(o => o.Product)
                .ThenInclude(p => p.Category)
                .Where(order=>order.UserId==userId)
                .ToListAsync();
            return order;
        }

        private string GetUser()
        {
            var principle = httpContextAccessor.HttpContext.User;
            var userId = userManager.GetUserId(principle);
            return userId;
        }
    }
}
