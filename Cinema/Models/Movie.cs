using Cinema.Models.enums;

namespace Cinema.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string TrailerUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public MovieStatus MovieStatus
        {
            get
            {
                if (DateTime.Now < StartDate)
                {
                    return MovieStatus.ComingSoon;
                }
                else if (DateTime.Now > EndDate)
                {
                    return MovieStatus.Expired;
                }
                else
                {
                    return MovieStatus.Available;
                }
            }


        }
       
        public int CategoryId { get; set; }
        public int CinemaId { get; set; }
        public ECinema Cinema { get; set; }
        public Category Category { get; set; }

        public List<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();

        public List<CartItems> CartItems { get; set; } = new List<CartItems>();     

    }
}
