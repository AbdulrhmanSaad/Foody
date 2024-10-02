using FoodOrdering.Dto;
using FoodOrdering.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrdering.User.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactUsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View("~/User/Views/ContactUs/Index.cshtml");
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]


        public IActionResult SendMessage(Contact data)
        {
            if (ModelState.IsValid)
            {
                data.CreatedDate = DateTime.Now;
                _context.Contacts.Add(data);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Your message has been sent successfully!";

                return RedirectToAction("Index"); 
            }
            return View("~/User/Views/ContactUs/Index.cshtml", data); 
            
        }
    }
}
