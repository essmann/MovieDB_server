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

    public DbSet<Actors> Actors { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<RatedMovie> RatedMovies { get; set; }
    public DbSet<WatchlistMovie> WatchlistMovies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actors>()
            .HasKey(e => new { e.MovieId, e.PersonId, e.Characters});

        modelBuilder.Entity<Directors>()
            .HasKey(e => new { e.MovieId, e.PersonId });

        modelBuilder.Entity<Writers>()
            .HasKey(e => new { e.MovieId, e.PersonId });
    }


}
