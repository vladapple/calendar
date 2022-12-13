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
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventMediaController : ControllerBase
    {
        private readonly CalendarDbContext _context;
        private readonly IConfiguration _config;

        public EventMediaController(CalendarDbContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }
        
        // GET: api/EventMedia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventMedia>>> GetEventMedias()
        {
            return await _context.EventMedias.Include(m => m.Event).ToListAsync();
        }

        // GET: api/EventMedia/event/5
        [HttpGet("event/{id}")]
        public async Task<ActionResult<IEnumerable<EventMedia>>> GetEventMediasByEvent(int id)
        {
            return await _context.EventMedias.Include(m => m.Event).Where(m => m.Event.Id == id).ToListAsync();
        }

        // GET: api/EventMedia/mediaType/image
        [HttpGet("mediaType")]
        public async Task<ActionResult<IEnumerable<EventMedia>>> GetEventMediasByMediaType(EventMedia.Medias type)
        {
            return await _context.EventMedias.Include(m => m.Event).Where(m => m.Media == type).ToListAsync();
        }

        // GET: api/EventMedia/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventMedia>> GetEventMedia(int id)
        {
            var eventMedia = await _context.EventMedias.Include(m => m.Event).Where(m => m.Id == id).FirstOrDefaultAsync();

            if (eventMedia == null)
            {
                return NotFound();
            }

            return eventMedia;
        }

        // PUT: api/EventMedia/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "user, admin")]
        public async Task<IActionResult> PutEventMedia(int id, EventMedia eventMedia)
        {
            if (id != eventMedia.Id)
            {
                return BadRequest();
            }

            var @event = _context.Events.FirstOrDefault(e => e.Id == eventMedia.Event.Id);
            eventMedia.Event = @event;

            _context.Entry(eventMedia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventMediaExists(id))
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

        // POST: api/EventMedia
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "user, admin")]
        public async Task<ActionResult<EventMedia>> PostEventMedia(EventMedia eventMedia)
        {
            var @event = _context.Events.FirstOrDefault(e => e.Id == eventMedia.Event.Id);
            eventMedia.Event = @event;

            _context.EventMedias.Add(eventMedia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventMedia", new { id = eventMedia.Id }, eventMedia);
        }

        //POST: api/EventMedia/upload
        [HttpPost("upload")]
        public async Task<IActionResult> PostUploadToAzure(IFormFile UploadFiles)
        {
            string connectionString = _config["AzureBlob"];
            string containerName = _config["Container"];
            string fileName = UploadFiles.FileName;

            BlobContainerClient container = new(connectionString, containerName);

            BlobClient blob = container.GetBlobClient(fileName);

            //blob.Upload(fileName);
            using (var fileStream = UploadFiles.OpenReadStream())
            {
                await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = UploadFiles.ContentType });
            }

            return Ok(container);
        }

        // DELETE: api/EventMedia/5
        [HttpDelete("{id}"), Authorize(Roles = "user, admin")]
        public async Task<IActionResult> DeleteEventMedia(int id)
        {
            var eventMedia = await _context.EventMedias.FindAsync(id);
            if (eventMedia == null)
            {
                return NotFound();
            }

            _context.EventMedias.Remove(eventMedia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventMediaExists(int id)
        {
            return _context.EventMedias.Any(e => e.Id == id);
        }
       
    }
}
