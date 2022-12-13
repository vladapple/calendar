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
    public class EventReviewsController : ControllerBase
    {
        private readonly CalendarDbContext _context;

        public EventReviewsController(CalendarDbContext context)
        {
            _context = context;
        }

        // GET: api/EventReviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventReview>>> GetEventReviews()
        {
            return await _context.EventReviews.Include(e => e.Event).Include(e => e.User).ToListAsync();
        }

        // GET: api/EventReviews/event/5
        [HttpGet("event/{id}")]
        public async Task<ActionResult<IEnumerable<EventReview>>> GetEventReviewsByEvent(int id)
        {
            return await _context.EventReviews.Include(e => e.Event).Include(e => e.User).Where(e => e.Event.Id == id).ToListAsync();
        }

        // GET: api/EventReviews/user/5
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<EventReview>>> GetEventReviewsByUser(int id)
        {
            return await _context.EventReviews.Include(e => e.Event).Include(e => e.User).Where(e => e.User.Id == id).ToListAsync();
        }

        // GET: api/EventReviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventReview>> GetEventReview(int id)
        {
            var eventReview = await _context.EventReviews.Include(e => e.Event).Include(e => e.User).Where(e => e.Id == id).FirstOrDefaultAsync();

            if (eventReview == null)
            {
                return NotFound();
            }

            return eventReview;
        }

        // PUT: api/EventReviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "user, admin")]
        public async Task<IActionResult> PutEventReview(int id, EventReview eventReview)
        {
            if (id != eventReview.Id)
            {
                return BadRequest();
            }

            var @event = _context.Events.FirstOrDefault(e => e.Id == eventReview.Event.Id);
            eventReview.Event = @event;
            var user = _context.Users.FirstOrDefault(u => u.Id == eventReview.User.Id);
            eventReview.User = user;

            _context.Entry(eventReview).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventReviewExists(id))
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

        // POST: api/EventReviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "user, admin")]
        public async Task<ActionResult<EventReview>> PostEventReview(EventReview eventReview)
        {
            var @event = _context.Events.FirstOrDefault(e => e.Id == eventReview.Event.Id);
            eventReview.Event = @event;
            var user = _context.Users.FirstOrDefault(u => u.Id == eventReview.User.Id);
            eventReview.User = user;
            _context.EventReviews.Add(eventReview);
            await _context.SaveChangesAsync();

            //review average injected into event table
            var eventReviewList = await _context.EventReviews.Include(e => e.Event).Where(e => e.Event.Id == @event.Id).ToListAsync();
            int sum = 0;
            int i = 0;
            foreach (var e in eventReviewList)
            {
                sum += e.Rating;
                i++;
            }
            decimal average = Convert.ToDecimal(sum) / i;
            @event.AverageRating = average;
            await _context.SaveChangesAsync();//review end

            return CreatedAtAction("GetEventReview", new { id = eventReview.Id }, eventReview);
        }

        // DELETE: api/EventReviews/5
        [HttpDelete("{id}"), Authorize(Roles = "user, admin")]
        public async Task<IActionResult> DeleteEventReview(int id)
        {
            var eventReview = await _context.EventReviews.FindAsync(id);
            if (eventReview == null)
            {
                return NotFound();
            }

            var @event = _context.Events.FirstOrDefault(e => e.Id == eventReview.Event.Id);
            eventReview.Event = @event;

            _context.EventReviews.Remove(eventReview);
            await _context.SaveChangesAsync();

            //review average injected into event table
            var eventReviewList = await _context.EventReviews.Include(e => e.Event).Where(e => e.Event.Id == @event.Id).ToListAsync();
            int sum = 0;
            int i = 0;
            foreach (var e in eventReviewList)
            {
                sum += e.Rating;
                i++;
            }
            decimal average = Convert.ToDecimal(sum) / i;
            @event.AverageRating = average;
            await _context.SaveChangesAsync();//review end

            return NoContent();
        }

        private bool EventReviewExists(int id)
        {
            return _context.EventReviews.Any(e => e.Id == id);
        }
    }
}
