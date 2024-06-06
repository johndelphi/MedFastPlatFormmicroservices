using System;
using System.Linq;
using System.Threading.Tasks;
using Medfast.Services.MedicationAPI.DbContexts;
using Medfast.Services.MedicationAPI.Models;
using Medfast.Services.MedicationAPI.Models.Dto;
using Medfast.Services.MedicationAPI.Repository.PharmacyRepository;
using Medfast.Services.MedicationAPI.Utility;
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
        private readonly EmailService _emailService;

        public AccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            JwtService jwtService,
            IPharmacyRepository pharmacyRepository, EmailService emailService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _pharmacyRepository = pharmacyRepository ?? throw new ArgumentNullException(nameof(pharmacyRepository));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
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
                var token = _jwtService.GenerateToken(user.Email, pharmacy.PharmacyName);
                return Ok(new { Token = token });
            }

            var errorMessages = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errorMessages });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
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

            var pharmacy = await _pharmacyRepository.GetPharmacyById((int)user.PharmacyId);
            if (pharmacy == null)
            {
                return NotFound($"Pharmacy with ID {user.PharmacyId} not found.");
            }

            // Pass the pharmacy name to GenerateToken
            var token = _jwtService.GenerateToken(user.Email, pharmacy.PharmacyName);
            return Ok(new { Token = token });
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("User with this email does not exist.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetCode = new Random().Next(100000, 999999).ToString(); // Generate a 6-digit code
            user.PasswordResetCode = resetCode;
            user.ResetTokenExpiration = DateTime.UtcNow.AddHours(1); // Set expiration time for the code

            await _context.SaveChangesAsync();

            // Send the code via email
            var emailSubject = "Password Reset Request";
            var emailBody = $"Your password reset code is: {resetCode}";
            await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);

            return Ok("Password reset code has been sent to your email.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || user.PasswordResetCode != model.ResetCode ||
                user.ResetTokenExpiration < DateTime.UtcNow)
            {
                return BadRequest("Invalid reset code or the code has expired.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);
            if (!resetResult.Succeeded)
            {
                var errorMessages = resetResult.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { Errors = errorMessages });
            }

            // Clear the reset code and expiration time after successful reset
            user.PasswordResetCode = null;
            user.ResetTokenExpiration = null;
            await _context.SaveChangesAsync();

            return Ok("Password has been reset successfully.");
        }
    }
}