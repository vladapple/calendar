using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenueReviewsController : ControllerBase
    {
        private readonly CalendarDbContext _context;

        public VenueReviewsController(CalendarDbContext context)
        {
            _context = context;
        }
       
        // GET: api/VenueReviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VenueReview>>> GetVenueReviews()
        {
            return await _context.VenueReviews.Include(v => v.Venue).Include(v => v.User).ToListAsync();
        }

        // GET: api/VenueReviews/venue/5
        [HttpGet("venue/{id}")]
        public async Task<ActionResult<IEnumerable<VenueReview>>> GetVenueReviewsByVenue(int id)
        {
            return await _context.VenueReviews.Include(v => v.Venue).Include(v => v.User).Where(v => v.Venue.Id == id).ToListAsync();
        }

        // GET: api/VenueReviews/user/5
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<VenueReview>>> GetVenueReviewsByUser(int id)
        {
            return await _context.VenueReviews.Include(v => v.Venue).Include(v => v.User).Where(v => v.User.Id == id).ToListAsync();
        }

        // GET: api/VenueReviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VenueReview>> GetVenueReview(int id)
        {
            var venueReview = await _context.VenueReviews.Include(v => v.Venue).Include(v => v.User).Where(v => v.Id == id).FirstOrDefaultAsync();

            if (venueReview == null)
            {
                return NotFound();
            }

            return venueReview;
        }

        // PUT: api/VenueReviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> PutVenueReview(int id, VenueReview venueReview)
        {
            if (id != venueReview.Id)
            {
                return BadRequest();
            }
            var venue = _context.Venues.FirstOrDefault(v => v.Id == venueReview.Venue.Id);
            venueReview.Venue = venue;
            var user = _context.Users.FirstOrDefault(u => u.Id == venueReview.User.Id);
            venueReview.User = user;
            _context.Entry(venueReview).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VenueReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/VenueReviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult<VenueReview>> PostVenueReview(VenueReview venueReview)
        {
            var venue = _context.Venues.FirstOrDefault(v => v.Id == venueReview.Venue.Id);
            venueReview.Venue = venue;
            var user = _context.Users.FirstOrDefault(u => u.Id == venueReview.User.Id);
            venueReview.User = user;
            _context.VenueReviews.Add(venueReview);
            await _context.SaveChangesAsync();

            //review average injected into venue table
            var venueReviewList = await _context.VenueReviews.Include(v => v.Venue).Where(v => v.Venue.Id == venue.Id).ToListAsync();
            int sum = 0;
            int i = 0;
            foreach (var v in venueReviewList)
            {
                sum += v.Rating;
                i++;
            }
            decimal average = Convert.ToDecimal(sum) / i;
            venue.AverageRating = average;
            await _context.SaveChangesAsync();
            //review end

            return CreatedAtAction("GetVenueReview", new { id = venueReview.Id }, venueReview);
        }

        // DELETE: api/VenueReviews/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> DeleteVenueReview(int id)
        {
            //var venueReview = await _context.VenueReviews.FindAsync(id);
            var venueReview = _context.VenueReviews.Include(v => v.Venue).FirstOrDefault(v => v.Id == id);
            var venue = _context.Venues.FirstOrDefault(v => v.Id == venueReview.Venue.Id);
            if (venueReview == null)
            {
                return NotFound();
            }

            _context.VenueReviews.Remove(venueReview);
            await _context.SaveChangesAsync();

            //review average injected into venue table
            var venueReviewList = await _context.VenueReviews.Include(v => v.Venue).Where(v => v.Venue.Id == venue.Id).ToListAsync();
            int sum = 0;
            int i = 0;
            foreach (var v in venueReviewList)
            {
                sum += v.Rating;
                i++;
            }
            decimal average = Convert.ToDecimal(sum) / i;
            venue.AverageRating = average;
            await _context.SaveChangesAsync();
            //review end

            return NoContent();
        }

        private bool VenueReviewExists(int id)
        {
            return _context.VenueReviews.Any(v => v.Id == id);
        }
    }
}
