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
    public class VenueCategoriesController : ControllerBase
    {
        private readonly CalendarDbContext _context;

        public VenueCategoriesController(CalendarDbContext context)
        {
            _context = context;
        }

        // *** GET MANY ***

        // GET: api/VenueCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VenueCategory>>> GetVenueCategories()
        {
            return await _context.VenueCategories.ToListAsync();
        }
  
        // *** GET ONE ***

        // GET: api/VenueCategories/2
        [HttpGet("{id}")]
        public async Task<ActionResult<VenueCategory>> GetVenueCategory(int id)
        {
            var venueCategory = await _context.VenueCategories.FindAsync(id);

            if (venueCategory == null)
            {
                return NotFound();
            }

            return venueCategory;
        }

        // PUT: api/VenueCategories/3
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutVenueCategory(int id, VenueCategory venueCategory)
        {
            if (id != venueCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(venueCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VenueCategoryExists(id))
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

        // POST: api/VenueCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<VenueCategory>> PostVenueCategory(VenueCategory venueCategory)
        {
            _context.VenueCategories.Add(venueCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVenueCategory", new { id = venueCategory.Id }, venueCategory);
        }

        // DELETE: api/VenueCategories/4
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVenueCategory(int id)
        {
            var venueCategory = await _context.VenueCategories.FindAsync(id);
            if (venueCategory == null)
            {
                return NotFound();
            }

            _context.VenueCategories.Remove(venueCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VenueCategoryExists(int id)
        {
            return _context.VenueCategories.Any(e => e.Id == id);
        }
    }
}