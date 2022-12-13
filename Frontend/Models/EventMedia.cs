using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
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


        public Event Event { get; set; }
    }
}
