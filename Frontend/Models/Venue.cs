using Frontend.Models;
using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class Venue
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100, ErrorMessage = "Venue Name is too long.")]
        public string VenueName { get; set; }

        [Required, StringLength(200, ErrorMessage = "Address is too long.")]
        public string Address { get; set; }

        [Required]
        public string Coordinates { get; set; }

        [Required, Range(5, 9000000000000, ErrorMessage = "Capacity must be at least 5 for us to call it a party.")]
        public int Capacity { get; set; }

        [Required]
        public bool WheelchairAccess { get; set; }

        [Required]
        public bool Food { get; set; }

        [Required]
        public bool Drink { get; set; }
        
        public decimal AverageRating { get; set; }

        public VenueCategory VenueCategory { get; set; }  

        public Venue()
        {
            WheelchairAccess = false;
            Food = false;
            Drink = false;
            AverageRating = 0;
        }   
    }
}

