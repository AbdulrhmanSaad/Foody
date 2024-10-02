using FoodOrdering.Constants;
using FoodOrdering.Dto;
using FoodOrdering.Models;
using FoodOrdering.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foody.Admin.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class AdminProductController : Controller
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        public AdminProductController(IProductService productService,ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            categoryService = categoryService;
        }
        public IActionResult Index()
        {
            List<Product> products = productService.GetAll();
            return View("~/Admin/Views/AdminProduct/Index.cshtml",products);
        }
        public IActionResult Details(int id)
        {
            return View("~/Admin/Views/AdminProduct/Details.cshtml",productService.GetById(id));
        }
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            ViewBag.Cateogries = categoryService.GetAll();
            if (ModelState.IsValid)
            {
                await productService.AddProduct(productDto);
                return RedirectToAction("Index");
            }
            return View("~/Admin/Views/AdminProduct/Create.cshtml", productDto);

        }
        public IActionResult Search(string name) 
        {

            List<Product> ps = productService.Search(name);
            return View("~/Admin/Views/AdminProduct/Index.cshtml", ps);
        }
        [HttpGet]
        public IActionResult Edit(int id) {
             var product = productService.GetById(id);
            ProductDto productDto = new()
            {
                Name = product.Name,
                Description = product.Description,
                IsActive = product.IsActive,
                Price = product.Price,
                Quantity = product.Quantity,
                CategoryId = product.CategoryId,
                ImgName = product.ImgeUrl
            };
            ViewBag.categories=categoryService.GetAll();
             return View("~/Admin/Views/AdminProduct/Edit.cshtml",productDto);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(int id,ProductDto product) {
            if (ModelState.IsValid)
            {
                await productService.Edit(id, product);
                return RedirectToAction("Index",productService.GetAll());
            }
            return View("~/Admin/Views/AdminProduct/Edit.cshtml",product);

        }
        public ActionResult Delete(int id) {
            productService.Delete(id);
          return RedirectToAction("Index");
        }


    }
}

