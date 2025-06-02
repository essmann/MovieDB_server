using System.ComponentModel.DataAnnotations;

namespace MovieDBMinimalAPI.Models
{
    public class WatchlistMovie {

            [Key]
            public string WatchlistId { get; set; }   
        
        public string UserId { get; set; }
        public string MovieId { get; set; }
        
        //navigation properties
        public User User { get; set; }
        public Movie movie { get; set; }
    }
}
