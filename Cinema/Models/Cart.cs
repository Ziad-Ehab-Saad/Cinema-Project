using Cinema.Identity;

namespace Cinema.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CartItems> CartItems { get; set; }
        public ApplicationUser  user{ get; set; }

    }
}
