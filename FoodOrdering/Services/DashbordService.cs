namespace FoodOrdering.Services
{
    public class DashbordService:IDashbordService
    {
        private readonly ApplicationDbContext context;

        public DashbordService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public StatisticData GetStatisticData()
        {
            var data= new StatisticData()
            {
                CategoriesCount=context.Categories.Count(),
                ProductsCount=context.Products.Count(),
                OrdersCount=context.Orders.Count(),
                UsersCount=context.Users.Count(),
                SoldAmount=context.Orders.Select(o=>o.TotalOrderPrice).Sum(),
            };
            return data;
        }
    }
}
