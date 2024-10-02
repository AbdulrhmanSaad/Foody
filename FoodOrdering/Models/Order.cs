namespace FoodOrdering.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }= DateTime.Now;
        public int OrderStatusId { get; set; }
        public double TotalOrderPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public bool IsDeleted { get; set; } 
        public List<OrderDetails> OrderDetails { get; set; }

       public int PaymentId { get; set; }
        public Paymant Paymant { get; set; }


    }
}
