using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Medfast.Services.MedicationAPI.DbContexts;
using Medfast.Services.MedicationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using BCrypt;

namespace Medfast.Services.MedicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if a user with the same email already exists
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                return BadRequest("User with this email already exists.");
            }

            // Validate model and create a new User object
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = model.Email,
                PasswordHash = HashPassword(model.Password) // Implement password hashing
            };

            // Save the new user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPost("pharmacyregistration")]
        public async Task<IActionResult> phamacyRegistration([FromBody] UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if a user with the same email already exists
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                return BadRequest("User with this email already exists.");
            }

            // Validate model and create a new User object
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = model.Email,
                PasswordHash = HashPassword(model.Password) // Implement password hashing
            };

            // Save the new user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }
        
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Add other authentication methods here (e.g., login, reset password, etc.)
    }
}
