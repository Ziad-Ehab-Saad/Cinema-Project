namespace Cinema.Models
{
    public class OrderItems
    {

        public int Id { get; set; }  // Primary Key

        // Foreign Key to Order
        public int OrderId { get; set; }
        public Order Order { get; set; }

        // Foreign Key to Movie
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int Quantity { get; set; }  // Number of this movie purchased
        public decimal Price { get; set; }  // Price of the movie at the time of purchase

    }
}
