using MovieDBMinimalAPI.Repository;
using MovieDBMinimalAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MovieDBMinimalAPI.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/users");

            group.MapGet("/ratings", async ([FromServices] IMovieRepository movieRepository,HttpContext ctx, IMovieApiProvider apiProvider) => 
         
            {
                if (!ctx.User.Identity.IsAuthenticated)
                {
                    return Results.Unauthorized();
                }


                var userId = ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Results.Unauthorized();
                }

                var movieList = await movieRepository.GetAllRatedMovies(userId);

                var movieDetailsTasks = movieList.Select(async ratedMovie =>
                {
                    var movie = await apiProvider.GetMovieByIdAsync(ratedMovie.MovieId);
                    return new RatedMovieDto
                    {
                        Movie = movie,
                        YourRating = ratedMovie.Rating,
                        DateRated = ratedMovie.RatedAt

                    };
                });

                var movieDetails = await Task.WhenAll(movieDetailsTasks);

                return Results.Ok(movieDetails);
            }); ;
            group.MapGet("/watchlist", async (IMovieRepository MovieRepository, HttpContext ctx) =>
            {

                var movieList = await MovieRepository.GetAllWatchlistMovies("blablatest");
                return movieList;

            });

            group.MapPost("/ratings/{movieId}/{rating}", async (
                int rating, string movieId, [FromServices] IMovieRepository MovieRepository, HttpContext ctx) =>
            {
                if (!ctx.User.Identity.IsAuthenticated)
                {
                    return Results.Unauthorized();
                }
                var _userId = ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (rating < 0 || rating > 10) return Results.BadRequest("Rating must be between 0 and 10.");

                try
                {
                    await MovieRepository.AddMovieRating(movieId, _userId, rating);
                    var response = new
                    {
                        Message = "Rating successfully added to database",
                        MovieId = movieId,
                        UserId = _userId,
                        Rating = rating
                    };

                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    // Log it
                    return Results.Problem("An error occurred while adding rating.");
                }
            });

        }
    }
}
