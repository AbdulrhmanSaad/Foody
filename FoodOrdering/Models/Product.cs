﻿namespace FoodOrdering.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string ImgeUrl { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public List<CartDetails> CartDetails { get; set; }
    }
}
