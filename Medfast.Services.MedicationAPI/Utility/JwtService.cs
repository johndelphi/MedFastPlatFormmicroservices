using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Medfast.Services.MedicationAPI.Utility
{
    public class JwtService
    {
        private readonly string _secretKey;
        private IEnumerable<ClaimsIdentity?> roles;
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null.");
            _secretKey = Environment.GetEnvironmentVariable("JwtSettings:SecretKey");
        }
        public string GenerateToken(string email, string pharmacyName, params string[] roles)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email), "Email cannot be null.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
            };

            // Include pharmacy name as a claim if provided
            if (!string.IsNullOrEmpty(pharmacyName))
            {
                claims.Add(new Claim("PharmacyName", pharmacyName));
            }

            // Add role claims if roles are provided
            if (roles != null && roles.Any())
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }




    }

}


