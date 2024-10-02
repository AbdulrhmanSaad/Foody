using Microsoft.AspNetCore.Mvc;

namespace FoodOrdering.User.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View("~/User/Views/Payment/Index.cshtml");
        }

        public IActionResult PaymentConfirmed()
        {

            return View("~/User/Views/Payment/PaymentConfirmed.cshtml");
        }
    }
}
