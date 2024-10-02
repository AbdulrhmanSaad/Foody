namespace FoodOrdering.Dto
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public IFormFile? Img { get; set; } = default!;

    }
}
