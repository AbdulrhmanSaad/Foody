using FoodOrdering.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrdering.Admin.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class ManageUsersController : Controller
    {
        private readonly IUserService _userService;

        public ManageUsersController(IUserService userService)
        {
             _userService = userService;
        }
        public IActionResult Index()
        {
            var users=_userService.GetAllUser();
            return View("~/Admin/Views/Users/Index.cshtml",users);
        }
        public IActionResult Delete(string id)
        {
            _userService.DeleteAccount(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
