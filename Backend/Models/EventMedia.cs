using Backend.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Backend.Models.User;

namespace Backend.Models
{
    public class EventMedia
    {
        [Key]
        public int Id { get; set; }

        [Required, EnumDataType(typeof(Medias))]
        public Medias Media { get; set; }

        public enum Medias
        {
            image,
            video,
            audio
        }

        [Required, Url]
        public string Url { get; set; }

        [Required]
        public Event Event { get; set; }
    }
}
