namespace FoodOrdering.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<CartDetails> CartDetails { get; set; }

    }
}
