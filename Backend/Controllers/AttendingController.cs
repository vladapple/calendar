using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using System.Net.Mail;
using System.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendingController : ControllerBase
    {
        private readonly CalendarDbContext _context;
        private readonly IConfiguration _config;

        public AttendingController(CalendarDbContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        // GET: api/Attendings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attending>>> GetAttendings()
        {
            return await _context.Attendings.Include(a => a.Event).Include(a => a.Event.Venue).Include(a => a.User).ToListAsync();
        }

        // GET: api/Attendings/event/5
        [HttpGet("event/{id}")]
        public async Task<ActionResult<IEnumerable<Attending>>> GetAttendingsByEvent(int id)
        {
            return await _context.Attendings.Include(a => a.Event).Include(a => a.Event.EventCategory).Include(a => a.Event.Venue).Include(a => a.Event.Venue.VenueCategory).Include(a => a.User).Where(a => a.Event.Id == id).ToListAsync();
        }

        // GET: api/Attendings/user/5
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Attending>>> GetAttendingsByUser(int id)
        {
            return await _context.Attendings.Include(a => a.Event).Include(a => a.Event.EventCategory).Include(a => a.Event.Venue).Include(a => a.Event.Venue.VenueCategory).Include(a => a.User).Where(a => a.User.Id == id).ToListAsync();
        }

        [HttpGet("userAsEvents/{id}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetAttendingsByUserAsEvents(int id)
        {
            var attendings = await _context.Attendings.Include(a => a.Event).Include(a => a.Event.EventCategory).Include(a => a.Event.Venue).Include(a => a.Event.Venue.VenueCategory).Include(a => a.User).Where(a => a.User.Id == id).ToListAsync();
            List<Event> events = new List<Event>();
            foreach (var a in attendings)
            {
                events.Add(a.Event);
            }
            return events;
    }

        // GET: api/Attendings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Attending>> GetAttending(int id)
        {
            var attending = await _context.Attendings.Include(a => a.Event).Include(a => a.Event.EventCategory).Include(a => a.Event.Venue).Include(a => a.Event.Venue.VenueCategory).Include(a => a.User).Where(a => a.Id == id).FirstOrDefaultAsync();

            if (attending == null)
            {
                return NotFound();
            }

            return attending;
        }

        // PUT: api/Attendings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "user, admin")]
        public async Task<IActionResult> PutAttending(int id, Attending attending)
        {
            if (id != attending.Id)
            {
                return BadRequest();
            }
            var @event = _context.Events.FirstOrDefault(e => e.Id == attending.Event.Id);
            attending.Event = @event;
            var user = _context.Users.FirstOrDefault(u => u.Id == attending.User.Id);
            attending.User = user;
            _context.Entry(attending).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendingExists(id))
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

        // POST: api/Attendings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "user, admin")]
        public async Task<ActionResult<Attending>> PostAttending(Attending attending)
        {
            var @event = _context.Events.FirstOrDefault(e => e.Id == attending.Event.Id);
            attending.Event = @event;
            var user = _context.Users.FirstOrDefault(u => u.Id == attending.User.Id);
            attending.User = user;
            _context.Attendings.Add(attending);
            await _context.SaveChangesAsync();

            //email
            var _gmailUser = _config["GmailUser"];
            var _gmailPassword = _config["GmailPassword"];
            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(_gmailUser),
                    Subject = "Attending Confirmation",
                    Body = $@"
                            <!DOCTYPE html>
                            <html>
                            <head>
                            <meta charset=""utf-8"" />
                            <title></title>
                            <body>
                            <h4 style='color:orrange'>Thank you for your participation!</h4><br> 
                            <h5>Here are details:</h5>
                            <p>Registered on: {DateTime.Now}<br>
                            Event: {@event.EventName}<br>
                            Event starts at: {@event.Start}<br>
                            Event ends at: {@event.End}<br>
                            Enjoy!<br><br>
                            Kind regards,<br>
                            Calendar</p>
                            </body></html>",
                    IsBodyHtml = true
                };
                mail.To.Add(user.Email);
                var client = new SmtpClient("smtp.gmail.com")
                {
                    UseDefaultCredentials = false,
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential(
                        _gmailUser, _gmailPassword),
                    EnableSsl = true
                };

                client.Send(mail);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("{0}: {1}", e.ToString(), e.Message);
            }
            //end email

            return CreatedAtAction("GetAttending", new { id = attending.Id }, attending);
        }

        // DELETE: api/Attendings/5
        [HttpDelete("{id}"), Authorize(Roles = "user, admin")]
        public async Task<IActionResult> DeleteAttending(int id)
        {
            var attending = await _context.Attendings.FindAsync(id);
            if (attending == null)
            {
                return NotFound();
            }

            _context.Attendings.Remove(attending);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AttendingExists(int id)
        {
            return _context.Attendings.Any(a => a.Id == id);
        }
    }
}
