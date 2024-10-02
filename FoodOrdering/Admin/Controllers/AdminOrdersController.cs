using FoodOrdering.Constants;
using FoodOrdering.Dto;
using FoodOrdering.Models;
using FoodOrdering.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodOrdering.Admin.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class AdminOrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public AdminOrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<IActionResult> DisplayAll()
        {
            var orders=await _orderService.GetOrders();
            return View("~/Admin/Views/AdminOrders/Index.cshtml",orders);
        }
        public IActionResult OrderDetails(int orderId)
        {
            var orders = _orderService.GetOrderDetails(orderId);
            return View("~/Admin/Views/AdminOrders/OrderDetails.cshtml",orders);
        }
        public async Task<IActionResult> UpdateOrderStatus(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
                throw new Exception("No order Found");

            var orderStatusList = (await _orderService.GetOrderStatuses()).Select(orderStatus =>
            {
                return new SelectListItem
                {
                    Value = orderStatus.Id.ToString(),
                    Text = orderStatus.StatusName,
                    Selected = orderStatus.Id == order.OrderStatusId
                };
            });
            var data = new UpdateOrderStatusModel()
            {
                OrderId = orderId,
                orderStatusId=order.OrderStatusId,
                OrderStatusNames=orderStatusList,

            };
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(UpdateOrderStatusModel data)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    var orderStatusList = (await _orderService.GetOrderStatuses()).Select(orderStatus =>
                //    {
                //        return new SelectListItem
                //        {
                //            Value = orderStatus.Id.ToString(),
                //            Text = orderStatus.StatusName,
                //            Selected = orderStatus.Id == data.OrderId
                //        };
                //    });
                //    return View(data);
                //}
                await _orderService.ChangeOrderStatus(data);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });

            }
            //return RedirectToAction(nameof(DisplayAll));

            
        }
    }
}
