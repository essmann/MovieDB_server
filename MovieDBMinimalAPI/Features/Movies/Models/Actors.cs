using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieDBMinimalAPI.Features.Movies.Models
{
    public class Actors
    {
        public string MovieId { get; set; }
        public string PersonId { get; set; }
        public string Characters { get; set; }

    }
}
