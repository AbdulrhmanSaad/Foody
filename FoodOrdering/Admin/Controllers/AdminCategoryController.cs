using FoodOrdering.Constants;
using FoodOrdering.Dto;
using FoodOrdering.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrdering.Admin.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class AdminCategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public AdminCategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        public IActionResult Index()
        {
            return View("~/Admin/Views/Category/Index.cshtml", categoryService.GetAll());
        }

        public async Task<IActionResult> Create(CategoryDto category)
        {
            if (ModelState.IsValid)
            {
                await categoryService.Create(category);
                return RedirectToAction("Index");

            }
            return View("~/Admin/Views/Category/Create.cshtml", category);

        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = categoryService.GetById(id);
            if (category == null)
                return NotFound();
            EditCategoryDto editCategory = new()
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ImgName = category.ImgeUrl
            };

            return View("~/Admin/Views/Category/Edit.cshtml", editCategory);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditCategoryDto category)
        {
            if (ModelState.IsValid)
            {
                await categoryService.Update(category);
                return RedirectToAction("Index");

            }
            return View("~/Admin/Views/Category/Edit.cshtml", category);
        }
        public IActionResult Delete(int id)
        {
            categoryService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
