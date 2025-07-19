
using MovieDBMinimalAPI.Features.Movies.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieDBMinimalAPI.Features.Movies.Models
{
    public class Movie
    {
        [Key]
        public string MovieId { get; set; } 

        public string? TitleType { get; set; }
        
        public string? OriginalTitle { get; set; }  
        public string? AlternateTitle { get; set; } 
        public bool isAdult { get; set; }
        public int startYear { get; set; }
        public int? endYear { get; set; }

        public int? RuntimeMinutes { get; set; }
        public decimal AverageRating { get; set; }
        public int NumVotes { get; set; }
        
        public ICollection<Actors> Actors { get; set; } //One-To-Many
        public ICollection<Directors> Directors { get; set; } //One-To-Many
        public ICollection<Writers> Writers { get; set; } //One-To-Many



        [NotMapped]
        public UserDataDto? UserData { get; set; }
    }
}
