
using Microsoft.EntityFrameworkCore;
using MovieDBMinimalAPI.Data;
using MovieDBMinimalAPI.Repository;
using MovieDBMinimalAPI.Services;

namespace MovieDBMinimalAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DB context
            builder.Services.AddDbContext<DbApplicationContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DI setup
            builder.Services.AddScoped<FetchApiMovies>();
            builder.Services.AddScoped<IMovieApiProvider, OmdbApiProvider>();
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddHttpClient<OmdbApiProvider>();
            builder.Services.AddLogging();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();




            app.MapGet("/movies/{id}", async (string id, IMovieApiProvider apiProvider) => //Program injects the correct fetchApiMovies implementation
            //via the builder.services.add thing
            {
                var response = await apiProvider.GetMovieByIdAsync(id);
                return response;
            });

            app.MapGet("/movies/search/{search}", async (string search, IMovieApiProvider apiProvider) =>
            {
                var response = await apiProvider.GetMoviesBySearchAsync(search);

                return response;
            });

            app.MapGet("/users/{userId}/watchlist", async (string userId, IMovieRepository MovieRepository) =>
            {
                var movieList = await MovieRepository.GetAllWatchlistMovies(userId);
                return movieList;

            });

            app.MapPost("/movies/{movieId}/{userId}/{rating}", async (
    int rating, string userId, string movieId, IFetchMoviesApi fetchApiMovies, IMovieRepository MovieRepository) =>
            {
                if (rating < 0 || rating > 10) return Results.BadRequest("Rating must be between 0 and 10.");

                try
                {
                    await MovieRepository.AddMovieRating(movieId, userId, rating);
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    // Log it
                    return Results.Problem("An error occurred while adding rating.");
                }
            });

            app.MapPost("/register", (string userId, IMovieRepository MovieRepository) => { 
                
            });
            //this takes a mandatory query parameter of search. so ?search=something

            app.Run();
            
        }
    }
}
