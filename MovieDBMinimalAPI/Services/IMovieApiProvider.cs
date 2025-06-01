using MovieDBMinimalAPI.Models;

namespace MovieDBMinimalAPI.Services
{
    public interface IMovieApiProvider
    {
        Task<Movie> GetMovieByIdAsync(string id);
        Task<List<Movie>> GetMoviesBySearchAsync(string query);
    }
}
