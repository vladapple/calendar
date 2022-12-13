using Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Venue
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(2), MaxLength(100)]
        public string VenueName { get; set; }

        [Required, MinLength(2), MaxLength(200)]
        public string Address { get; set; }

        [Required]
        public string Coordinates { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public bool WheelchairAccess { get; set; }

        [Required]
        public bool Food { get; set; }

        [Required]
        public bool Drink { get; set; }

        public decimal AverageRating { get; set; }

        [Required]
        public VenueCategory VenueCategory { get; set; }  
    }
}

