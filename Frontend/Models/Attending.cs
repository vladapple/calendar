using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class Attending
    {
        [Key]
        public int Id { get; set; }

        public User User { get; set; }
        
        public Event Event { get; set; }
    }
}
