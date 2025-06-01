using System.Runtime.CompilerServices;
using System.Text.Json;
using MovieDBMinimalAPI.Data;
using MovieDBMinimalAPI.Models;

namespace MovieDBMinimalAPI.Services
{
    public class OmdbApiProvider : IMovieApiProvider
    {
        private const string baseApiUrl = "http://www.omdbapi.com/";
        private const string apiKey = "25ed2cf3";
        private const string fullApiUrl = $"{baseApiUrl}?apikey={apiKey}";

        private HttpClient _httpClient;
        private DbApplicationContext _context;

        public class SearchResults
        {
            public List<Movie> Search {  get; set; }
        }
        public OmdbApiProvider(HttpClient httpClient, DbApplicationContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<Movie> GetMovieByIdAsync(string id)
        {
            string newUrl = fullApiUrl + $"&i={id}";
            var response = await _httpClient.GetAsync(newUrl);
            response.EnsureSuccessStatusCode(); // optional, throws if status != 200

            var json = await response.Content.ReadAsStringAsync();
            var movie = JsonSerializer.Deserialize<Movie>(json);
            return movie;
        }
        
        public async Task<List<Movie>> GetMoviesBySearchAsync(string search)
        {
            string newUrl = fullApiUrl + $"&s={search}";
            var response = await _httpClient.GetAsync(newUrl);
            response.EnsureSuccessStatusCode(); // optional, throws if status != 200

            var json = await response.Content.ReadAsStringAsync();
            
            
            var results = JsonSerializer.Deserialize<SearchResults>(json);
            
            return results.Search;
        }
    }
}
