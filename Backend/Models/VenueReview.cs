using Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Backend.Models
{
    public class VenueReview
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(2000), AllowNull]
        public string Comment { get; set; }

        [Required, Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public Venue Venue { get; set; }
    }
}
