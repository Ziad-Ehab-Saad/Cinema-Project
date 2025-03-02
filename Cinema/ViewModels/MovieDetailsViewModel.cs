using Cinema.Models;

namespace Cinema.ViewModels
{
    public class MovieDetailsViewModel
    {
        public Movie  movie{ get; set; }

        public List<Actor> Actors { get; set; }
    }
}
