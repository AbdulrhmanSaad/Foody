namespace FoodOrdering.Dto
{
    public class PaymentDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ExpireDate { get; set; }
        public string? CardNo { get; set; }
        public int CvvNo { get; set; }
        public int PaymentMethod { get; set; }



    }
}
