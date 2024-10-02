using FoodOrdering.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrdering.Admin.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class AdminController : Controller
    {
        private readonly IDashbordService _dashbordService;

        public AdminController(IDashbordService dashbordService)
        {
            _dashbordService = dashbordService;
        }
        public IActionResult Index()
        {
            var data=_dashbordService.GetStatisticData();
            return View("~/Admin/Views/Index.cshtml",data);
        }
    }
}
