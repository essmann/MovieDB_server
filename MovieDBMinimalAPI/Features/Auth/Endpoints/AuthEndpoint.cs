using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using MovieDBMinimalAPI.Repository;
using MovieDBMinimalAPI.Services;
using System.Security.Claims;

using MovieDBMinimalAPI.Features.Auth.DTO;
using MovieDBMinimalAPI.Features.Users.Services;
namespace MovieDBMinimalAPI.Features.Auth.Endpoints
{
    public static class AuthEndpoint
    {

        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/auth");


            group.MapPost("/login", async (HttpContext ctx, IUserRepository userRepository) =>
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
            group.MapGet("/logout", async (HttpContext ctx) =>
            {
                await ctx.SignOutAsync();
                return "logged out";
            });
        }
    }
}
