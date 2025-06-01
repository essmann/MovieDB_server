using Microsoft.EntityFrameworkCore;
using MovieDBMinimalAPI.Models;

namespace MovieDBMinimalAPI.Data
{
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



    }

}


