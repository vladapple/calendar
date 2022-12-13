using Backend.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Backend.Data
{
    public class CalendarDbContext : DbContext
    {
        public CalendarDbContext(DbContextOptions<CalendarDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Attending> Attendings { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventMedia> EventMedias { get; set; }
        public DbSet<EventReview> EventReviews { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<VenueCategory> VenueCategories { get; set; }
        public DbSet<VenueMedia> VenueMedias { get; set; }
        public DbSet<VenueReview> VenueReviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "TestUser",
                    Password = BCrypt.Net.BCrypt.HashPassword("Test123!"),
                    ContactName = "",
                    OrganizationName = "",
                    IsBanned = false,
                    Email = "User@Test.com",
                    DOB = DateTime.UtcNow.AddYears(-18),
                    Role = User.Roles.user

                },
                new User
                {
                    Id = 2,
                    UserName = "TestAdmin",
                    Password = BCrypt.Net.BCrypt.HashPassword("Test123!"),
                    ContactName = "",
                    OrganizationName = "",
                    IsBanned = false,
                    Email = "Admin@Test.com",
                    DOB = DateTime.UtcNow.AddYears(-18),
                    Role = User.Roles.admin

                });

            modelBuilder.Entity<EventCategory>().HasData(
                new EventCategory
                {
                    Id = 1,
                    CategoryName= "Concert",
                    Descr = "Live music"
                },
                new EventCategory
                {
                    Id = 2,
                    CategoryName = "Play",
                    Descr = "Theatrical presentation"
                },
                new EventCategory
                {
                    Id = 3,
                    CategoryName = "Art Exhibition",
                    Descr = "Viewing of art pieces"
                },
                new EventCategory
                {
                    Id = 4,
                    CategoryName = "Festival",
                    Descr = "Thematic celebration, often over multiple days"
                },
                new EventCategory
                {
                    Id = 5,
                    CategoryName = "Party",
                    Descr = "General gathering of people"
                },
                new EventCategory
                {
                    Id = 6,
                    CategoryName = "Fundraiser",
                    Descr = "Event aimed at collecting money for a cause"
                });

            modelBuilder.Entity<VenueCategory>().HasData(
                new VenueCategory
                {
                    Id = 1,
                    CategoryName = "Bar",
                    Descr = "Drinking establishment"
                },
                new VenueCategory
                {
                    Id = 2,
                    CategoryName = "Theatre",
                    Descr = "Area for dramatic performances"
                },
                new VenueCategory
                {
                    Id = 3,
                    CategoryName = "Gallery",
                    Descr = "Venue for displaying art"
                },
                new VenueCategory
                {
                    Id = 4,
                    CategoryName = "Park",
                    Descr = "Open green space for recreation"
                },
                new VenueCategory
                {
                    Id = 5,
                    CategoryName = "Stadium",
                    Descr = "Large capacity arena for popular events"
                },
                new VenueCategory
                {
                    Id = 6,
                    CategoryName = "Restaurant",
                    Descr = "Location for dining"
                },
                new VenueCategory
                {
                    Id = 7,
                    CategoryName = "Hall",
                    Descr = "Large multi-purpose gathering space"
                });

            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
    
}
