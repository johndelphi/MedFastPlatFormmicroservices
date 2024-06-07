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
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            JwtService jwtService, IPharmacyRepository pharmacyRepository, EmailService emailService, ILogger<AccountController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _pharmacyRepository = pharmacyRepository ?? throw new ArgumentNullException(nameof(pharmacyRepository));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for registration.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Registering user with email: {Email}", model.Email);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("User with email {Email} already exists.", model.Email);
                return Conflict("User with this email already exists.");
            }

            Pharmacy pharmacy;
            try
            {
                pharmacy = await _pharmacyRepository.GetPharmacyById(model.PharmacyId);
                if (pharmacy == null)
                {
                    _logger.LogWarning("Pharmacy with ID {PharmacyId} not found.", model.PharmacyId);
                    return BadRequest("Invalid pharmacy ID.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pharmacy with ID {PharmacyId}", model.PharmacyId);
                return StatusCode(500, "Internal server error.");
            }

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                UserName = model.username,
                PhoneNumber = model.PhoneNumber,
                PharmacyId = model.PharmacyId,
                PasswordResetCode = null,  // Ensure this is set to null
                ResetTokenExpiration = null  // Ensure this is set to null
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Generate JWT token
                var token = _jwtService.GenerateToken(user.Email, pharmacy.PharmacyName);
                _logger.LogInformation("User registered successfully. Email: {Email}", user.Email);
                return Ok(new { Token = token });
            }

            var errorMessages = result.Errors.Select(e => e.Description).ToList();
            _logger.LogError("User registration failed. Errors: {Errors}", string.Join(", ", errorMessages));
            return BadRequest(new { Errors = errorMessages });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for login.");
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                _logger.LogWarning("Invalid login attempt for email: {Email}", model.Email);
                return Unauthorized("Invalid email or password.");
            }

            Pharmacy pharmacy;
            try
            {
                pharmacy = await _pharmacyRepository.GetPharmacyById((int)user.PharmacyId);
                if (pharmacy == null)
                {
                    _logger.LogWarning("Pharmacy with ID {PharmacyId} not found.", user.PharmacyId);
                    return NotFound($"Pharmacy with ID {user.PharmacyId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pharmacy with ID {PharmacyId}", user.PharmacyId);
                return StatusCode(500, "Internal server error.");
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
                _logger.LogWarning("Invalid model state for password reset request.");
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogWarning("Password reset requested for non-existing email: {Email}", model.Email);
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
                _logger.LogWarning("Invalid model state for password reset.");
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || user.PasswordResetCode != model.ResetCode ||
                user.ResetTokenExpiration < DateTime.UtcNow)
            {
                _logger.LogWarning("Invalid password reset code for email: {Email}", model.Email);
                return BadRequest("Invalid reset code or the code has expired.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);
            if (!resetResult.Succeeded)
            {
                var errorMessages = resetResult.Errors.Select(e => e.Description).ToList();
                _logger.LogError("Password reset failed for email: {Email}. Errors: {Errors}", model.Email, string.Join(", ", errorMessages));
                return BadRequest(new { Errors = errorMessages });
            }

            // Clear the reset code and expiration time after successful reset
            user.PasswordResetCode = null;
            user.ResetTokenExpiration = null;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Password reset successfully for email: {Email}", model.Email);
            return Ok("Password has been reset successfully.");
        }
    }
}