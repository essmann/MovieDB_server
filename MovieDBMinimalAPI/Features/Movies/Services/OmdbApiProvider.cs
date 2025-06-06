﻿using System.Runtime.CompilerServices;
using System.Text.Json;
using MovieDBMinimalAPI.Data;
using MovieDBMinimalAPI.Features.Movies.Models;

namespace MovieDBMinimalAPI.Services
{
    public class RatedMovieDto
    {
        public required Movie Movie { get; set; }
        public required double YourRating { get; set; }
        public required DateOnly DateRated { get; set; }
    }
    public class OmdbApiProvider : IMovieApiProvider
    {
        private const string baseApiUrl = "http://www.omdbapi.com/";
       

        private HttpClient _httpClient;
        private DbApplicationContext _context;
        private string apiKey;
        private string fullApiUrl;
        public class SearchResults
        {
            public List<Movie> Search {  get; set; }
        }
        public OmdbApiProvider(HttpClient httpClient, DbApplicationContext context, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _context = context;
            apiKey = configuration["ApiKey"];
            fullApiUrl  = $"{baseApiUrl}?apikey={apiKey}";
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
