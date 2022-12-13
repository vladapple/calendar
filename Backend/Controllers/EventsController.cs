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
using System.Globalization;
using System.Collections;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly CalendarDbContext _context;

        public EventsController(CalendarDbContext context)
        {
            _context = context;
        }

        // *** GET MANY ***

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            await ChangeStatus();
            return await _context.Events.Include(e => e.Venue).Include(e => e.Venue.VenueCategory).Include(e => e.User).Include(e => e.EventCategory).ToListAsync();
        }

        // GET: api/Events/venue/5
        [HttpGet("venue/{id}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByVenue(int id)
        {
            await ChangeStatus();
            return await _context.Events.Include(e => e.Venue).Include(e => e.Venue.VenueCategory).Include(e => e.User).Include(e => e.EventCategory).Where(e => e.Venue.Id == id).ToListAsync();
        }

        // GET: api/Events/user/5
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByUser(int id)
        {
            await ChangeStatus();
            return await _context.Events.Include(e => e.Venue).Include(e => e.Venue.VenueCategory).Include(e => e.User).Include(e => e.EventCategory).Where(e => e.User.Id == id).ToListAsync();
        }

        // GET: api/Events/category/5
        [HttpGet("category/{id}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByCategory(int id)
        {
            await ChangeStatus();
            return await _context.Events.Include(e => e.Venue).Include(e => e.Venue.VenueCategory).Include(e => e.User).Include(e => e.EventCategory).Where(e => e.EventCategory.Id == id).ToListAsync();
        }

        // GET: api/Events/approval/pending
        [HttpGet("approval")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByApprovalStatus(Event.Approvals status)
        {
            await ChangeStatus();
            return await _context.Events.Include(e => e.Venue).Include(e => e.Venue.VenueCategory).Include(e => e.User).Include(e => e.EventCategory).Where(e => e.ApprovalStatus == status).ToListAsync();
        }

        //all approved events with current status = upcoming and live ordered by date
        [HttpGet("approved")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByApproved()
        {
            await ChangeStatus();
            return await _context.Events.Include(e => e.Venue).Include(e => e.Venue.VenueCategory).Include(e => e.User).Include(e => e.EventCategory).Where(e => e.ApprovalStatus == Event.Approvals.approved).Where(e => e.CurrentStatus != Event.Current.completed).Where(e => e.CurrentStatus != Event.Current.cancelled).OrderBy(e => e.Start).ToListAsync();
        }

        //all approved events that are live(today).
        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByToday()
        {
            await ChangeStatus();
            return await _context.Events.Include(e => e.Venue).Include(e => e.Venue.VenueCategory).Include(e => e.User).Include(e => e.EventCategory).Where(e => e.ApprovalStatus == Event.Approvals.approved).Where(e => e.CurrentStatus == Event.Current.live).Where(e => e.CurrentStatus != Event.Current.cancelled).OrderBy(e => e.Start).ToListAsync();
        }

        //all approved events that are next week.
        [HttpGet("nextWeek")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByNextWeek()
        {
            int thisWeek = Week(DateTime.Now);
            int nextWeek;
            if(thisWeek == 52)
            {
                nextWeek = 1;
            }
            else
            {
                nextWeek = thisWeek + 1;
            }
            await ChangeStatus();
            var eventList = await _context.Events.Include(e => e.Venue).Include(e => e.Venue.VenueCategory).Include(e => e.User).Include(e => e.EventCategory).Where(e => e.ApprovalStatus == Event.Approvals.approved).Where(e => e.CurrentStatus == Event.Current.upcoming).Where(e => e.CurrentStatus != Event.Current.cancelled).OrderBy(e => e.Start).ToListAsync();
            List<Event> newEventList = new List<Event>();
            foreach(var e in eventList)
            {
                if(Week(e.Start) == nextWeek)
                {
                    newEventList.Add(e);
                } 
            }
            return newEventList;
        }

        //all approved events that are next month.
        [HttpGet("nextMonth")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByNextMonth()
        {
            int nextMonth = 0;
            if (DateTime.Now.Month == 12)
            {
                nextMonth = 1;
            }
            else
            {
                nextMonth = (DateTime.Now).Month + 1;
            }
            await ChangeStatus();
            return await _context.Events.Include(e => e.Venue).Include(e => e.Venue.VenueCategory).Include(e => e.User).Include(e => e.EventCategory).Where(e => e.ApprovalStatus == Event.Approvals.approved).Where(e => e.CurrentStatus != Event.Current.completed).Where(e => e.CurrentStatus != Event.Current.cancelled).Where(e => e.Start.Month == nextMonth).OrderBy(e => e.Start).ToListAsync();
        }

        //all approved events that are on weekends.
        [HttpGet("weekend")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByWeekend()
        {
            await ChangeStatus();
            return await _context.Events.Include(e => e.Venue).Include(e => e.Venue.VenueCategory).Include(e => e.User).Include(e => e.EventCategory).Where(e => e.ApprovalStatus == Event.Approvals.approved).Where(e => e.CurrentStatus != Event.Current.completed).Where(e => e.CurrentStatus != Event.Current.cancelled).Where(e => e.Start.DayOfWeek == DayOfWeek.Saturday || e.Start.DayOfWeek == DayOfWeek.Sunday).OrderBy(e => e.Start).ToListAsync();
        }

        // GET: api/Events/current/live
        [HttpGet("current")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByCurrentStatus(Event.Current status)
        {
            await ChangeStatus();
            return await _context.Events.Include(e => e.Venue).Include(e => e.Venue.VenueCategory).Include(e => e.User).Include(e => e.EventCategory).Where(e => e.CurrentStatus == status).Where(e => e.ApprovalStatus == Event.Approvals.approved).OrderBy(e => e.Start).ToListAsync();
        }

        // *** GET ONE ***

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            await ChangeStatus();
            var @event = await _context.Events.Include(e => e.Venue).Include(e => e.Venue.VenueCategory).Include(e => e.User).Include(e => e.EventCategory).Where(e => e.Id == id).FirstOrDefaultAsync();

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "user, admin")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            if (id != @event.Id)
            {
                return BadRequest();
            }

            var category = _context.EventCategories.FirstOrDefault(c => c.Id == @event.EventCategory.Id);
            @event.EventCategory = category;

            var venue = _context.Venues.FirstOrDefault(v => v.Id == @event.Venue.Id);
            @event.Venue = venue;

            var user = _context.Users.FirstOrDefault(u => u.Id == @event.User.Id);
            @event.User = user;

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost/*, Authorize(Roles = "user, admin")*/]
        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {
            var category = _context.EventCategories.FirstOrDefault(c => c.Id == @event.EventCategory.Id);
            @event.EventCategory = category;

            var venue = _context.Venues.FirstOrDefault(v => v.Id == @event.Venue.Id);
            @event.Venue = venue;

            var user = _context.Users.FirstOrDefault(u => u.Id == @event.User.Id);
            @event.User = user;

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}"), Authorize(Roles = "user, admin")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        private async Task ChangeStatus()
        {
            //start change Current status in the database
            List<Event> events = await _context.Events.Where(e => e.ApprovalStatus == Event.Approvals.approved).Where(e => e.CurrentStatus != Event.Current.cancelled).Where(e => e.CurrentStatus != Event.Current.completed).ToListAsync();
            foreach (var e in events)
            {
                if (e.Start > DateTime.Now)
                {
                    e.CurrentStatus = Event.Current.upcoming;
                }
                if (e.Start <= DateTime.Now && e.End >= DateTime.Now)
                {
                    e.CurrentStatus = Event.Current.live;
                }
                if (e.End < DateTime.Now)
                {
                    e.CurrentStatus = Event.Current.completed;
                }
            }
            await _context.SaveChangesAsync();
            //end
        }
        private int Week(DateTime date)
        {
            CultureInfo myCI = new CultureInfo("en-CA");
            Calendar myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            // Displays the total number of weeks in the current year.
            DateTime dateFormat = new System.DateTime(date.Year, date.Month, date.Day);
            int weekNumber = myCal.GetWeekOfYear(dateFormat, myCWR, myFirstDOW);
            return weekNumber;
        }
    }
}
