using MovieDBMinimalAPI.Models;
namespace MovieDBMinimalAPI.DTO
{

    public class MovieWithUserDataDto
    {
        public Movie movie { get; set; }

        public UserDataDto UserData { get; set; }



    }
    public class UserDataDto
    {
               public int? UserRating { get; set; }
        public DateOnly? DateRated { get; set; }
        public bool IsInWatchlist { get; set; }
        public DateOnly? DateWatchlist { get; set; }
    }
}
