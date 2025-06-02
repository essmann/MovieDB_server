using Microsoft.EntityFrameworkCore;
using MovieDBMinimalAPI.Models;

namespace MovieDBMinimalAPI.Repository
{
    public interface IMovieRepository
    {
        Task<IEnumerable<RatedMovie>> GetAllRatedMovies(string userId);
        Task<IEnumerable<WatchlistMovie>> GetAllWatchlistMovies(string userId);

        Task AddMovieRating(string movieId, string userId, int rating);
        Task AddMovieToWatchlist(string movieId, string userId);

        Task AddMovie(Movie movie);
        Task<bool> MovieExistsAsync(string movieId);
        
            
        
        Task SaveChangesAsync(); // For unit of work if desired
    }
}

