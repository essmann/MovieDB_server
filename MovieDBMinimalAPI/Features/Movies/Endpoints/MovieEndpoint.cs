using Microsoft.AspNetCore.Mvc;
using MovieDBMinimalAPI.Features.Movies.Repository;
using MovieDBMinimalAPI.Repository;
using MovieDBMinimalAPI.Services;
using System.Security.Claims;
using MovieDBMinimalAPI.Features.Movies.DTO;

namespace MovieDBMinimalAPI.Features.Movies.Endpoints;

public static class MovieEndpoint
{
    public static void MapMovieEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/movie");

        group.MapGet("/{id}", async (string id, [FromServices] IMovieApiProvider apiProvider, HttpContext ctx, IMovieRepository movieRepository, bool includeUserData=false) =>
        {
            var response = await apiProvider.GetMovieByIdAsync(id);
            MovieWithUserDataDto? newResponse = null;
            if (ctx.User.Identity.IsAuthenticated && includeUserData)
            {
                var userId = ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var ratedMovie = await movieRepository.UserHasRatedMovieAsync(id, userId);
                var watchlistMovie = await movieRepository.UserHasAddedMovieToWatchlistAsync(id, userId);
                response.UserData = new UserDataDto
                {
                    DateRated = ratedMovie?.RatedAt,
                    UserRating = ratedMovie?.Rating ?? null,
                    DateWatchlist = watchlistMovie?.AddedAt,
                    IsInWatchlist = watchlistMovie != null
                };



                return Results.Ok(newResponse);
            }

            return Results.Ok(response);
        });

       

        group.MapGet("/search/{search}", async (string search, IMovieApiProvider apiProvider) =>
        {
            var response = await apiProvider.GetMoviesBySearchAsync(search);

            return response;
        });
        group.MapGet("/ratings", async ([FromServices] IMovieRepository movieRepository, HttpContext ctx, IMovieApiProvider apiProvider) =>

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
