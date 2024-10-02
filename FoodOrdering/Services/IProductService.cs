namespace FoodOrdering.Services
{
    public interface IProductService
    {
        Task AddProduct(ProductDto product);
        List<Product> GetAll();
        Product GetById(int id);
        List<Product> GetByCategoryId(int categoryId);
        void Delete(int id);
        Task Edit(int id, ProductDto product);
        List<Product> Search(string name);
    }
}
