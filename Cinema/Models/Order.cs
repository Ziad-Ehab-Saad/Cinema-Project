using Cinema.Models.enums;

namespace Cinema.Models
{
    public class Order
    {
        public int Id { get; set; }  

        public string UserId { get; set; }  
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string? PaymentIntentId { get; set; } // Stripe Payment Intent ID
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Navigation Property: One order can have multiple OrderItems
        public List<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

    }
}
