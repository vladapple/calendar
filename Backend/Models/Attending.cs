using Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Attending
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public Event Event { get; set; }
    }
}
