using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.Dtos;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize(Roles = "admin, user")]
    public class UsersController : ControllerBase
    {
        private readonly CalendarDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(CalendarDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Users
        [HttpGet/*, Authorize(Roles = "admin, user")*/]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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


        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        [HttpPost("register"), AllowAnonymous]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            if (request.Username == string.Empty || request.Email == string.Empty || request.Password == string.Empty) return BadRequest("Required fields are missing (Username, email, Password");
            //TODO: Validate Email, DOB, etc
            if (_context.Users.Any(e => e.UserName == request.Username) || _context.Users.Any(e => e.Email == request.Email)) return BadRequest("Username/Email not unique");
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            User newUser = new User( request.Username, request.Email, hashedPassword, request.DOB, request.ContactName, request.OrganizationName);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new {Message ="User registered"});

        }
        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<string>> Login(UserLogin request)
        {
            if(!_context.Users.Any(e => e.UserName == request.UserName))
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(e => e.UserName == request.UserName);
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest("Invalid Login");
            }
            else
            {
                
                return Ok(CreateToken(user)); //TODO: Implement Token
            }

        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.SerialNumber, user.Id.ToString()),

            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("JWTKey").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        [HttpGet("validate"), Authorize]
        public ActionResult<object> ValidateToken()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.SerialNumber);

            return Ok(new { username, role, userId });
        }

    }
}
