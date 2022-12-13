using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Backend.Models
{
    public class EventCategory
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(2), MaxLength(100)]
        public string CategoryName { get; set; }

        [MaxLength(2000), AllowNull]
        public string Descr { get; set; }
    }
}
