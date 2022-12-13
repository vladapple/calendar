using Backend.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class VenueMedia
    {
        [Key]
        public int Id { get; set; }

        [Required, EnumDataType(typeof(Mediasv))]
        public Mediasv Media { get; set; }

        public enum Mediasv
        {
            image,
            video,
            audio
        }

        [Required, Url]
        public string Url { get; set; }

        [Required]
        public Venue Venue { get; set; }
    }
}
