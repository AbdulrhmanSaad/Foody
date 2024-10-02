
namespace FoodOrdering.Dto
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? Img { get; set; } = default!;
        public string? ImgName { get; set; }
    }
}
