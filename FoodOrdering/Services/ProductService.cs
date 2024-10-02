namespace FoodOrdering.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagePath;

        public ProductService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagePath = $"{_webHostEnvironment.WebRootPath}{FileSetting.ImageProductPath}";

        }

        public async Task AddProduct(ProductDto product)
        {
           Product p=new Product();
           p.Name = product.Name;
            p.Description = product.Description;
            p.CreatedDate= DateTime.Now;
            p.CategoryId= product.CategoryId;
            p.IsActive= product.IsActive;
            p.Price= product.Price;
            p.Quantity= product.Quantity;
            p.ImgeUrl =await SaveCover(product.Img);
            context.Products.Add(p);
            context.SaveChanges();

        }


        public List<Product> GetAll()
        {
            return context.Products.Include(p=>p.Category).OrderByDescending(p=>p.CategoryId).ThenBy(p=>p.Name).ToList();
        }

        public List<Product> GetByCategoryId(int categoryId)
        {
            return context.Products.Where(p=>p.CategoryId==categoryId).OrderBy(p=>p.Name).ToList();
        }

        public Product GetById(int id)
        {
            return context.Products.Include(p=>p.Category).FirstOrDefault(p => p.Id == id);
        }

        public void Delete(int id)
        {
            context.Products.Remove(GetById(id));
            context.SaveChanges();
        }

        public async Task Edit(int id,ProductDto product)
        {
           Product oldProduct=GetById(id);
            var oldImgName = oldProduct.ImgeUrl;
           oldProduct.Name= product.Name;
            oldProduct.Description= product.Description;
            oldProduct.Price= product.Price;
            oldProduct.Quantity= product.Quantity;
            oldProduct.CategoryId= product.CategoryId;
            oldProduct.IsActive = product.IsActive;


            if (product.Img is not null) 
            {
                //save new img 
                oldProduct.ImgeUrl=await SaveCover(product.Img);
            }
            context.Update(oldProduct);
            var effectedRows= context.SaveChanges();
            if (effectedRows > 0)
            {
                //delete old image
                var name=Path.Combine(_imagePath,oldImgName);
                File.Delete(name);
            }
            else
            {
                var name = Path.Combine(_imagePath, oldProduct.ImgeUrl);
                File.Delete(name);
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

        public List<Product> Search(string name)
        {
           return context.Products.Where(p=>p.Name.StartsWith(name)).AsNoTracking().ToList();
        }

       
    }
}
