using FoodOrdering.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrdering.User.Controllers
{
    [Authorize]
    public class UserOrderController : Controller
    {
        private readonly IOrderService _orderService;

        public UserOrderController(IOrderService orderService)
        {
           _orderService = orderService;
        }
        public async Task<IActionResult> Index()
        {
            var orders=await _orderService.GetUserOrders();
            return View("~/User/Views/UserOrder/Index.cshtml",orders);
        }
    }
}
