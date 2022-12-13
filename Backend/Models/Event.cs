using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Backend.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(2), MaxLength(100)]
        public string EventName { get; set; }

        [MaxLength(2000), AllowNull]
        public string Descr { get; set; }

        [Required, Range(typeof(DateTime), "1/1/2020", "1/1/2100")]
        public DateTime Start { get; set; }

        [Required, Range(typeof(DateTime), "1/1/2020", "1/1/2100")]
        public DateTime End { get; set; }

        [Required]
        public bool AgeRestricted { get; set; }

        [Required]
        public decimal TicketPrice { get; set; }

        [EnumDataType(typeof(Approvals)), DefaultValue(Approvals.pending)]
        public Approvals ApprovalStatus { get; set; }

        public enum Approvals
        {
            pending,
            approved,
            rejected
        }

        [EnumDataType(typeof(Current)), DefaultValue(Current.upcoming)]
        public Current CurrentStatus { get; set; }

        public enum Current
        {
            upcoming,
            live,
            completed,
            cancelled
        }

        public decimal AverageRating { get; set; }

        [Required]
        public Venue Venue { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public EventCategory EventCategory { get; set; }
    }
}