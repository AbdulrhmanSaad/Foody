using FoodOrdering.Dto;
using FoodOrdering.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace FoodOrdering.User.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddItem(int productId,int qyt = 1,int redirct=0)
        {
           var itemsCount=await _cartService.AddItem(productId, qyt);
            if (redirct == 0) {
                return Ok(itemsCount);

                //if (itemsCount)
                //{
                //    return Json(new { success = true, message = "Item added to cart successfully!" });
                //}
                //else
                //{
                //    return Json(new { success = false, message = "Failed to add item to cart." });
                //}
            }
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> Remove(int productId)
        {
            var itemsCount = await _cartService.RemoveItem(productId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()//int productId
        {
            var cart = await _cartService.GetUserCart();
            return View("~/User/Views/Cart/GetUserCart.cshtml", cart);
        }
        public async Task<IActionResult> GetCartCount()
        {
            var cartCount =await _cartService.GetCartItemsCount();
            return Ok(cartCount);
        }
        [HttpGet]
        public ActionResult CheckOut()
        {
            return View("~/User/Views/Cart/Index.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> CheckOut(PaymentDto payment,int method)
        {
            if(!ModelState.IsValid) 
                return View("~/User/Views/Cart/Index.cshtml", payment);

            payment.PaymentMethod = method;
            if (method == 1)
            {
                var isSuccessful =_cartService.CheckOutCOD(payment);
                if (isSuccessful)
                    return RedirectToAction("PaymentConfirmed", "Payment");
                else
                    throw new Exception("SomeThing went wrong");
                  
            }
            else
            {
                Session session =await _cartService.CheckOut(payment);
                if (session == null)
                    throw new Exception("SomeThing went wrong");

                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            //return RedirectToAction("Index", "Home");


        }
 
    }
}
