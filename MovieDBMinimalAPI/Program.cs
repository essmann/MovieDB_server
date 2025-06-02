
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieDBMinimalAPI.Data;
using MovieDBMinimalAPI.Repository;
using MovieDBMinimalAPI.Services;
using MovieDBMinimalAPI.Models;

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

            //Authentication setup
            builder.Services.AddAuthorization();
           

            // DI setup
            builder.Services.AddScoped<FetchApiMovies>();
            builder.Services.AddScoped<IMovieApiProvider, OmdbApiProvider>();
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddHttpClient<OmdbApiProvider>();
            builder.Services.AddLogging();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000", "https://movie-dj7e5iw1b-essmanns-projects.vercel.app")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });

            });

            //equivalent implementation



            builder.Services.AddAuthentication()
              .AddCookie("cookie", (o) =>
              {

                  o.Cookie.Name = "MovieDB";
                  o.ExpireTimeSpan = TimeSpan.FromHours(8);
                  o.SlidingExpiration = true;

              });


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            //Authentication endpoints

            app.MapPost("/login", async (HttpContext ctx, IUserRepository userRepository) =>
            {

                var request = await ctx.Request.ReadFromJsonAsync<GoogleLoginRequest>();



                string jwt = request.Jwt;
                GoogleJsonWebSignature.Payload payload;
                try
                {
                    payload = await GoogleJsonWebSignature.ValidateAsync(jwt, new GoogleJsonWebSignature.ValidationSettings
                    {
                        Audience = new[] { "621707536726-6sigj02j4qqu4t8upatok2ocsp3etg88.apps.googleusercontent.com" }
                    });
                }
                catch
                {
                    return Results.Unauthorized();
                }

                //Check if User exists in database, if not, register the user.
                var userExists = await userRepository.UserExistsAsync(payload.Subject);
                if (!userExists)
                {
                    DateOnly today = DateOnly.FromDateTime(DateTime.Now);
                    await userRepository.UserAddAsync(payload.Subject, payload.Email, today, payload.Name);
                }
                    // Create claims from payload
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, payload.Subject),
                        new Claim(ClaimTypes.Email, payload.Email),
                        new Claim(ClaimTypes.Name, payload.Name ?? payload.Email)
                    };

                var identity = new ClaimsIdentity(claims, "cookie");
                var principal = new ClaimsPrincipal(identity);

                await ctx.SignInAsync(principal); // Sign in with cookie scheme

                return Results.Ok("Logged in");
            });
            app.MapGet("/logout", async (HttpContext ctx) =>
            {
                await ctx.SignOutAsync();
                return "logged out";
            });

            app.MapGet("/profile", async (HttpContext ctx) => {

                if (!ctx.User.Identity.IsAuthenticated)
                {
                    return Results.Unauthorized(); //401
                }
                var user = ctx.User;
                var name = user.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
                var email = user.FindFirst(ClaimTypes.Email)?.Value ?? "Unknown";
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown";

                return Results.Ok(new
                {
                    Name = name,
                    Email = email,
                    UserId = userId
                });

            });
            //Movie endpoints

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

            app.MapGet("/users/watchlist", async (IMovieRepository MovieRepository, HttpContext ctx) =>
            {

                var movieList = await MovieRepository.GetAllWatchlistMovies("blablatest");
                return movieList;

            });

            app.MapPost("/users/rating/{movieId}/{rating}", async (
    int rating, string userId, string movieId, IFetchMoviesApi fetchApiMovies, IMovieRepository MovieRepository, HttpContext ctx) =>
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
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    // Log it
                    return Results.Problem("An error occurred while adding rating.");
                }
            });

            
            //this takes a mandatory query parameter of search. so ?search=something

            app.Run();
            
        }
    }
}
