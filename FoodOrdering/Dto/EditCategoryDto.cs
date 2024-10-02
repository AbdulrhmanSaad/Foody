namespace FoodOrdering.Dto
{
    public class EditCategoryDto:CategoryDto
    {
        public int Id { get; set; }
        public string? ImgName {  get; set; }
    }
}
