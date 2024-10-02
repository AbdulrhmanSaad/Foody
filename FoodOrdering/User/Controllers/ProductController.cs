using FoodOrdering.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrdering.User.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetProductsByCategory(int categoryId)
        {

            //List<Product> products=context.Products.Where(p=>p.CategoryId == categoryId).ToList();
            return Json(productService.GetByCategoryId(categoryId));
        }

        public IActionResult GetAll()
        {
            return View("~/User/Views/Product/GetAll.cshtml", productService.GetAll());
        }

        public IActionResult Details(int id)
        {
            return View("~/User/Views/Product/Details.cshtml",productService.GetById(id));
        }
    }
}
