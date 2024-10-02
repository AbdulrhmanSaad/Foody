namespace FoodOrdering.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        //add image
        public string ImgeUrl { get; set; }
    }
}
