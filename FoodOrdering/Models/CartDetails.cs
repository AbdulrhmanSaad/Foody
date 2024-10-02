namespace FoodOrdering.Models
{
    public class CartDetails
    {
        public int Id { get; set; }
        public int ShoppingCartId { get; set; }
        public ShoppingCart shoppingCart { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
        public int Quantity { get; set; }

    }
}
