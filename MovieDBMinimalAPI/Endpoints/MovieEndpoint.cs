using Microsoft.AspNetCore.Mvc;
using MovieDBMinimalAPI.Repository;
using MovieDBMinimalAPI.Services;
using System.Security.Claims;

namespace MovieDBMinimalAPI.Endpoints
{
    public static class MovieEndpoint
    {
        public static void MapMovieEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/movie");

            group.MapGet("/{id}", async (string id, [FromServices] IMovieApiProvider apiProvider, HttpContext ctx) =>
            {
                var response = await apiProvider.GetMovieByIdAsync(id);


                return Results.Ok(response);
            });

            group.MapGet("/{id}/includeUserInfo", async (
                      string id,
                      HttpContext ctx,
                      IMovieService movieService) =>
            {
                if (ctx.User.Identity?.IsAuthenticated == true)
                {
                    var userId = ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var movieWithUserData = await movieService.GetMovieWithOptionalUserDataAsync(id, userId);
                    return Results.Ok(movieWithUserData);
                }

                return Results.Unauthorized();
            });
            group.MapGet("/{id}/includeUserInfo/test", async (
          string id,

          IMovieService movieService) =>
            {

                var userId = "118262629380429584236";
                var movieWithUserData = await movieService.GetMovieWithOptionalUserDataAsync(id, userId);
                return Results.Ok(movieWithUserData);


                return Results.Unauthorized();
            });

            group.MapGet("/search/{search}", async (string search, IMovieApiProvider apiProvider) =>
            {
                var response = await apiProvider.GetMoviesBySearchAsync(search);

                return response;
            });


        }
    }
}
