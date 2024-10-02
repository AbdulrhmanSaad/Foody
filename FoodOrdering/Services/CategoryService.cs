namespace FoodOrdering.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagePath;

        public CategoryService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagePath = $"{_webHostEnvironment.WebRootPath}{FileSetting.ImageCateogryPath}";

        }

        public async Task Create(CategoryDto category)
        {
            Category newCategory = new Category();
            newCategory.Name = category.Name;
            newCategory.CreatedDate = DateTime.Now;
            newCategory.IsActive = category.IsActive;
            newCategory.ImgeUrl = await SaveCover(category.Img);
            context.Add(newCategory);
            context.SaveChanges();
        }
        
        

        public void Delete(int id)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == id);
            context.Remove(category);
            context.SaveChanges();
        }

        public List<Category> GetAll()
        {
            return context.Categories.ToList();
        }

       

        public Category GetById(int id)
        {
            return context.Categories.Find(id);
        }

        public async Task Update(EditCategoryDto category)
        {
            if(category != null)
            {
                var oldCategory=context.Categories.FirstOrDefault(c=>c.Id == category.Id);
                var oldCover = oldCategory.ImgeUrl;
                oldCategory.Name=category.Name;
                oldCategory.IsActive=category.IsActive;
                if(category.Img is not null)
                {
                    //save new photo
                     oldCategory.ImgeUrl = await SaveCover(category.Img);
                }

                context.Update(oldCategory);
                var effectedRows = context.SaveChanges();
                if (effectedRows > 0)
                {
                    //delete old photo
                    if(category.Img is not null)
                    {
                        var cover=Path.Combine(_imagePath, oldCover);
                        File.Delete(cover);
                    }
                }
                else
                {
                    //delete the new photo if changes not applied
                    var cover=Path.Combine(_imagePath,oldCategory.ImgeUrl);
                    File.Delete(cover);
                }
            }
        }
        public async Task<string> SaveCover(IFormFile img)
        {
            var imgName = $"{Guid.NewGuid()}{Path.GetExtension(img.FileName)}";
            var path = Path.Combine(_imagePath, imgName);
            using var stream = File.Create(path);
            await img.CopyToAsync(stream);
            return imgName;
        }

       
    }
}
