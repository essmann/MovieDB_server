using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using MovieDBMinimalAPI.Features.Movies.Models;

namespace MovieDBMinimalAPI
{
   
  

    public class SearchedMovie
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Poster { get; set; }
        public string imdbID { get; set; }
        public string Type { get; set; }



    }
 
    public class SearchResults
    {
        public List<SearchedMovie> Search { get; set; }
       
    }
   
  
    public interface IFetchMoviesApi
    {  //Contract for how movies should be fetched, regardless of which API.
        Task<Movie> GetSingleMovie(string id);
        Task<SearchResults> GetMovies(string search);

        
    }
    public class FetchApiMovies : IFetchMoviesApi
    {
        private const string baseApiUrl = "http://www.omdbapi.com/";
        private const string apiKey = "25ed2cf3";
        private const string fullApiUrl =  $"{baseApiUrl}?apikey={apiKey}";
        private readonly HttpClient _httpClient;
        
       
        public FetchApiMovies(HttpClient httpclient) {
        
            _httpClient = httpclient;
        }

        public async Task<Movie> GetSingleMovie(string id) {

            string newUrl = fullApiUrl + $"&i={id}";
            var response = await _httpClient.GetAsync(newUrl);
            response.EnsureSuccessStatusCode(); // optional, throws if status != 200

            var json = await response.Content.ReadAsStringAsync();
            var movie = JsonSerializer.Deserialize<Movie>(json);
            return movie;
        }

        public async Task<SearchResults> GetMovies(string search) {


            string newUrl = fullApiUrl + $"&s={search}";
            var response = await _httpClient.GetAsync(newUrl);
            response.EnsureSuccessStatusCode(); // optional, throws if status != 200

            var json = await response.Content.ReadAsStringAsync();
            var results= JsonSerializer.Deserialize<SearchResults>(json);
            return results;
        }
    }
}
