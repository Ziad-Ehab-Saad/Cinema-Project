namespace Cinema.Models
{
    public class CartItems
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int MovieId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Cart  cart { get; set; }
        public Movie movie { get; set; }

    }
}
