namespace FoodOrdering.Models
{
    public class Paymant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string? ExpireDate { get; set; }
        public string? CardNo { get; set; }
        public int CvvNo { get; set; }
        public string Address { get; set; }
        public int PaymentMode { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

    }
}
