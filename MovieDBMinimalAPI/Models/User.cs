using System.ComponentModel.DataAnnotations;

namespace MovieDBMinimalAPI.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string Email { get; set; }
        public DateOnly DateRegisteredAt { get; set; }

        public ICollection<WatchlistMovie> WatchlistMovies { get; set; }
        public ICollection<RatedMovie> RatedMovies { get; set; }
    }
}
