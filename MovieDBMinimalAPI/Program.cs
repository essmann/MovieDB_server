using Azure;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using MovieDBMinimalAPI.Data;
using MovieDBMinimalAPI.Features.Auth.DTO;
using MovieDBMinimalAPI.Features.Auth.Endpoints;

using MovieDBMinimalAPI.Features.Movies.Endpoints;
using MovieDBMinimalAPI.Features.Movies.Repository;
using MovieDBMinimalAPI.Features.Users.Services;
using MovieDBMinimalAPI.Repository;
using MovieDBMinimalAPI.Services;
using System.Security.Claims;

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
                        .WithOrigins("http://localhost:3000", "https://movie-db-ten-tawny.vercel.app")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });

            });

            //equivalent implementation



            builder.Services.AddAuthentication()
                .AddCookie("cookie", options =>
                {
                    options.Cookie.Name = "MovieDB";
                    options.Cookie.SameSite = SameSiteMode.None;  // Allow cross-site cookie
                    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Only send over HTTPS
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                    options.SlidingExpiration = true;
                });


            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors();
            //app.UseHttpsRedirection();
            var test = false;
            if (test)
            {
                app.Use(async (context, next) =>
                {
                    var identity = new ClaimsIdentity(new[]
                    {
            new Claim(ClaimTypes.NameIdentifier, "118262629380429584236"),
            new Claim(ClaimTypes.Name, "Essmann"),
            new Claim(ClaimTypes.Email, "kenwilliam1000@gmail.com"),
        }, "FakeAuthentication");

                    context.User = new ClaimsPrincipal(identity);
                    await next();
                });
            }
            app.UseAuthentication();
            app.UseAuthorization();

            //mapping our endpoint files to actual endpoints in the app            
            app.MapAuthEndpoints();
            //app.MapUserEndpoints();
            app.MapMovieEndpoints();


            //Authentication endpoints
            app.MapGet("/", () => "Hello from MovieDB API!");

            app.MapGet("/profile", async (HttpContext ctx) =>
            {

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




            //this takes a mandatory query parameter of search. so ?search=something

            app.Run();

        }
    }
}
