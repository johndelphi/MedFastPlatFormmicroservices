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
using Microsoft.AspNetCore.Identity;

namespace Medfast.Services.MedicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return Conflict("User with this email already exists.");
            }

            var user = new ApplicationUser
            {
               
                Email = model.Email,
                PasswordHash = model.Password,
                 UserName = model.username,
              
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status201Created);
            }

            // If registration fails, return detailed error messages
            var errorMessages = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errorMessages });




        }
    }
}
