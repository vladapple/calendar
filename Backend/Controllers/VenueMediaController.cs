using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http.Features;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenueMediaController : ControllerBase
    {
        private readonly CalendarDbContext _context;
        private readonly IConfiguration _config;

        public VenueMediaController(CalendarDbContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }
        
        // GET: api/VenueMedia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VenueMedia>>> GetVenueMedias()
        {
            return await _context.VenueMedias.Include(v => v.Venue).ToListAsync();
        }

        
        // GET: api/VenueMedia/venue/5
        [HttpGet("venue/{id}")]
        public async Task<ActionResult<IEnumerable<VenueMedia>>> GetVenueMediasByVenue(int id)
        {
            return await _context.VenueMedias.Include(v => v.Venue).Where(v => v.Venue.Id == id).ToListAsync();
        }

        // GET: api/VenueMedia/mediaType/image
        [HttpGet("mediaType")]
        public async Task<ActionResult<IEnumerable<VenueMedia>>> GetVenueMediasByMediaType(VenueMedia.Mediasv type)
        {
            return await _context.VenueMedias.Include(v => v.Venue).Where(v => v.Media == type).ToListAsync();
        }
        
       
        // GET: api/VenueMedia/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VenueMedia>> GetVenueMedia(int id)
        {
            var venueMedia = await _context.VenueMedias.Include(v => v.Venue).Where(v => v.Id == id).FirstOrDefaultAsync();

            if (venueMedia == null)
            {
                return NotFound();
            }

            return venueMedia;
        }
        
        
        // PUT: api/VenueMedia/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVenueMedia(int id, VenueMedia venueMedia)
        {
            if (id != venueMedia.Id)
            {
                return BadRequest();
            }

            var venue = _context.Venues.FirstOrDefault(v => v.Id == venueMedia.Venue.Id);
            venueMedia.Venue = venue;

            _context.Entry(venueMedia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VenueMediaExists(id))
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
       
        
        // POST: api/VenueMedia
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VenueMedia>> PostVenueMedia(VenueMedia venueMedia)
        {
            var venue = _context.Venues.FirstOrDefault(v => v.Id == venueMedia.Venue.Id);
            venueMedia.Venue = venue;

            _context.VenueMedias.Add(venueMedia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVenueMedia", new { id = venueMedia.Id }, venueMedia);
        }

        //POST: api/VenueMedia/upload
        [HttpPost("upload")]
        public async Task<IActionResult> PostUploadToAzure(IFormFile UploadFiles)
        {
            string connectionString = _config["AzureBlob"];
            string containerName = _config["Container"];
            string fileName = UploadFiles.FileName;

            BlobContainerClient container = new (connectionString, containerName);

            BlobClient blob = container.GetBlobClient(fileName);

            //blob.Upload(fileName);
            using (var fileStream = UploadFiles.OpenReadStream())
            {
                await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = UploadFiles.ContentType });
            }

            return Ok(container);
        }

        // DELETE: api/VenueMedia/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenueMedia(int id)
        {
            var venueMedia = await _context.VenueMedias.FindAsync(id);
            if (venueMedia == null)
            {
                return NotFound();
            }

            _context.VenueMedias.Remove(venueMedia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VenueMediaExists(int id)
        {
            return _context.VenueMedias.Any(v => v.Id == id);
        }
    }
}
