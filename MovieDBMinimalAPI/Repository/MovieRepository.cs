using Microsoft.EntityFrameworkCore;
using MovieDBMinimalAPI.Data;
using MovieDBMinimalAPI.Models;

namespace MovieDBMinimalAPI.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly FetchApiMovies fetchApiMovies;
        private readonly DbApplicationContext _context;

        public MovieRepository(DbApplicationContext context, FetchApiMovies fetchApiMovies)
        {
            _context = context;
            this.fetchApiMovies = fetchApiMovies;
        }

        public async Task<IEnumerable<RatedMovie>> GetAllRatedMovies(string userId)
        {
            return await _context.RatedMovies
                                 .Where(rm => rm.UserId == userId)
                                 .ToListAsync();
        } 

        public async Task<IEnumerable<WatchlistMovie>> GetAllWatchlistMovies(string userId)
        {
            return await _context.WatchlistMovies
                                 .Where(wm => wm.UserId == userId)
                                 .ToListAsync();
        }
        public async Task AddMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> MovieExistsAsync(string movieId)
        {
            return await _context.Movies.AnyAsync(m => m.imdbID == movieId);
        }
        public async Task AddMovieRating(string movieId, string userId, int rating)
        {
           
           
             
                    bool canConnect = await _context.Database.CanConnectAsync();
                    var count = await _context.RatedMovies.CountAsync();
                    bool movieAlreadyRated = await _context.RatedMovies.AnyAsync(rm => rm.MovieId == movieId && rm.UserId == userId);
                
              
            
            

            if (!movieAlreadyRated)
            {
                string id = Guid.NewGuid().ToString();
                var newRating = new RatedMovie
                {
                    RatingId = id,
                    MovieId = movieId,
                    UserId = userId,
                    Rating = rating
                };
                _context.RatedMovies.Add(newRating);
                
            }
            if(! await MovieExistsAsync(movieId))
            {
               var movie = await  fetchApiMovies.GetSingleMovie(movieId);
                movie.MyRating = rating.ToString();
                await AddMovie(movie);
            }
            await _context.SaveChangesAsync();
        }

        public async Task AddMovieToWatchlist(string movieId, string userId)
        {


            bool exists = await _context.WatchlistMovies.AnyAsync(wm => wm.MovieId == movieId && wm.UserId == userId);
            
            if (!exists)
            {
                var newItem = new WatchlistMovie
                {
                    MovieId = movieId,
                    UserId = userId,
                };
                _context.WatchlistMovies.Add(newItem);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
