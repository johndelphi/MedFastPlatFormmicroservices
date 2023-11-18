using System;
using System.Threading.Tasks;
using Medfast.Services.MedicationAPI.DbContexts;
using Medfast.Services.MedicationAPI.Models;
using Medfast.Services.MedicationAPI.Models.Dto;
using Medfast.Services.MedicationAPI.Repository.PharmacyRepository;
using Medfast.Services.MedicationAPI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Medfast.Services.MedicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService _jwtService;
        private readonly IPharmacyRepository _pharmacyRepository;

        public AccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, JwtService jwtService,
            IPharmacyRepository pharmacyRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _pharmacyRepository = pharmacyRepository ?? throw new ArgumentNullException(nameof(pharmacyRepository));
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

            var pharmacy = await _pharmacyRepository.GetPharmacyById(model.PharmacyId);
            if (pharmacy == null)
            {
                return BadRequest("Invalid pharmacy ID.");
            }

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.username,
                PhoneNumber = model.PhoneNumber,
                PharmacyId = pharmacy.PharmacyId,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Generate JWT token
                var token = _jwtService.GenerateToken(user.Email);
                return Ok(new { Token = token });
            }

            var errorMessages = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errorMessages });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = _jwtService.GenerateToken(user.Email);
            return Ok(new { Token = token });
        }
    }
}
