using System.ComponentModel.DataAnnotations;

namespace MovieDBMinimalAPI.Models
{
    public class RatedMovie
    {
        [Key]
        public string RatingId { get; set; }   
        public string MovieId { get; set; }
        
        public string UserId { get; set; }
        public int Rating { get; set; }

        //navigation properties
        public Movie movie { get; set; }
        public User User{ get; set; }

    }
}
