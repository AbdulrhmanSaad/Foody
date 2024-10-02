namespace FoodOrdering.Services
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        Task Create(CategoryDto category);
        void Delete(int id);
        Category GetById(int id);
        Task Update(EditCategoryDto category);
    }
}
