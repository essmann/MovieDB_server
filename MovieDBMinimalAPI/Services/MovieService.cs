using MovieDBMinimalAPI.DTO;
using MovieDBMinimalAPI.Repository;
using MovieDBMinimalAPI.Models;

namespace MovieDBMinimalAPI.Services
{
    // Services/MovieService.cs
    public class MovieService : IMovieService
    {
        private readonly IMovieApiProvider _apiProvider;
        private readonly IMovieRepository _movieRepo;
        //private UserDataDto userData;

        public MovieService(IMovieApiProvider apiProvider, IMovieRepository movieRepo)
        {
            _apiProvider = apiProvider;
            _movieRepo = movieRepo;
        }

        public async Task<MovieWithUserDataDto> GetMovieWithOptionalUserDataAsync(string movieId, string? userId)
        {
            var movie = await _apiProvider.GetMovieByIdAsync(movieId);

            UserDataDto? userData = null;  // ✅ define it early

            if (!string.IsNullOrEmpty(userId))
            {
                var ratedMovies = await _movieRepo.GetAllRatedMovies(userId);
                var ratedMovie = ratedMovies.FirstOrDefault(rm => rm.MovieId == movie.imdbID && rm.UserId == userId);

                var watchlistMovies = await _movieRepo.GetAllWatchlistMovies(userId);
                var watchlistMovie = watchlistMovies.FirstOrDefault(wm => wm.MovieId == movie.imdbID && wm.UserId == userId);

                userData = new UserDataDto
                {
                    UserRating = ratedMovie?.Rating,
                    DateRated = ratedMovie?.RatedAt,
                    DateWatchlist = watchlistMovie?.AddedAt,
                    IsInWatchlist = watchlistMovie != null
                };
            }

            return new MovieWithUserDataDto
            {
                movie = movie,
                UserData = userData  // ✅ safely nullable
            };
        }
    }

}
