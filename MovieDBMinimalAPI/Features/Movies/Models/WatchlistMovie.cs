using MovieDBMinimalAPI.Features.Users.Models;
using System.ComponentModel.DataAnnotations;
namespace MovieDBMinimalAPI.Features.Movies.Models
{
    public class WatchlistMovie
    {

        [Key]
        public string WatchlistId { get; set; }

        public string UserId { get; set; }
        public string MovieId { get; set; }
        public DateOnly AddedAt { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        //navigation properties
        public User User { get; set; }
        public Movie movie { get; set; }
    }
}
