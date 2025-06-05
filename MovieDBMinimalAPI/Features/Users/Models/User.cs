using MovieDBMinimalAPI.Features.Movies.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieDBMinimalAPI.Features.Users.Models
{
    public class User
    {
        [Key]

        public required string Id { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public DateOnly DateRegisteredAt { get; set; }

        public ICollection<WatchlistMovie> WatchlistMovies { get; set; }
        public ICollection<RatedMovie> RatedMovies { get; set; }
    }
}
