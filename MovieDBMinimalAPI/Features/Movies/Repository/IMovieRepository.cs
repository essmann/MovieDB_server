using Microsoft.EntityFrameworkCore;
using MovieDBMinimalAPI.Features.Movies.Models;

namespace MovieDBMinimalAPI.Features.Movies.Repository;

public interface IMovieRepository
{
    Task<IEnumerable<RatedMovie>> GetAllRatedMovies(string userId);
    Task<IEnumerable<WatchlistMovie>> GetAllWatchlistMovies(string userId);

    Task AddMovieRating(string movieId, string userId, int rating);
    Task AddMovieToWatchlist(string movieId, string userId);

    Task AddMovie(Movie movie);
    Task<bool> MovieExistsAsync(string movieId);

    Task<RatedMovie> UserHasRatedMovieAsync(string movieId, string userId);
    Task<WatchlistMovie> UserHasAddedMovieToWatchlistAsync(string movieId, string userId);
    

    Task SaveChangesAsync(); // For unit of work if desired
}

