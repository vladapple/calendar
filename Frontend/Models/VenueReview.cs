using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Frontend.Models
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

        public User User { get; set; }

        public Venue Venue { get; set; }

        public VenueReview()
        {
            Comment = "";
        }       
    }
}
