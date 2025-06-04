using MovieDBMinimalAPI.DTO;
using MovieDBMinimalAPI.Models;
namespace MovieDBMinimalAPI.Services
{
    public interface IMovieService
    {
        Task<MovieWithUserDataDto> GetMovieWithOptionalUserDataAsync(string movieId, string? userId);
    }
}
