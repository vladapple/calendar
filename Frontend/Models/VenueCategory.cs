using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Frontend.Models
{
    public class VenueCategory
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(3, ErrorMessage = "Should be more than 2 carachters"), MaxLength(100, ErrorMessage = "Should be less than 101 carachters")]
        public string CategoryName { get; set; }

        [MaxLength(2000), AllowNull]
        public string Descr { get; set; }

        public VenueCategory()
        {
            Descr = "";
        }
    }
}