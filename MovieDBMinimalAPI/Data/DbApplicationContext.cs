using Microsoft.EntityFrameworkCore;
using MovieDBMinimalAPI.Features.Movies.Models;
using MovieDBMinimalAPI.Features.Users.Models;

namespace MovieDBMinimalAPI.Data;

public class DbApplicationContext : DbContext
{
    public DbApplicationContext(DbContextOptions<DbApplicationContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RatedMovie> RatedMovies { get; set; }
    public DbSet<WatchlistMovie> WatchlistMovies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data (example 2 movies)
        modelBuilder.Entity<Movie>().HasData(
            new Movie
            {
                imdbID = "tt0111161",
                Title = "The Shawshank Redemption",
                Year = "1994",
                Rated = "R",
                Released = "14 Oct 1994",
                Runtime = "142 min",
                Genre = "Drama",
                Director = "Frank Darabont",
                Writer = "Stephen King, Frank Darabont",
                Actors = "Tim Robbins, Morgan Freeman",
                Plot = "Two imprisoned men bond over a number of years...",
                Language = "English",
                Country = "USA",
                Awards = "Nominated for 7 Oscars. Another 21 wins & 36 nominations.",
                Poster = "https://example.com/poster1.jpg",
                Metascore = "80",
                imdbRating = "9.3",
                imdbVotes = "2,500,000",
                Type = "movie",
                DVD = "27 Jan 1998",
                BoxOffice = "$28,341,469",
                Production = "Castle Rock Entertainment",
                Website = "N/A",
                Response = "True"
            },
            new Movie
            {
                imdbID = "tt0068646",
                Title = "The Godfather",
                Year = "1972",
                Rated = "R",
                Released = "24 Mar 1972",
                Runtime = "175 min",
                Genre = "Crime, Drama",
                Director = "Francis Ford Coppola",
                Writer = "Mario Puzo, Francis Ford Coppola",
                Actors = "Marlon Brando, Al Pacino",
                Plot = "The aging patriarch of an organized crime dynasty...",
                Language = "English, Italian",
                Country = "USA",
                Awards = "Won 3 Oscars. Another 26 wins & 30 nominations.",
                Poster = "https://example.com/poster2.jpg",
                Metascore = "100",
                imdbRating = "9.2",
                imdbVotes = "1,800,000",
                Type = "movie",
                DVD = "09 Oct 2001",
                BoxOffice = "$134,966,411",
                Production = "Paramount Pictures",
                Website = "N/A",
                Response = "True"
            }
        );
    }
}
