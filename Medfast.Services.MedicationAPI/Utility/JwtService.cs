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

        public JwtService(string secretKey)
        {
            _secretKey = secretKey;
        }
        public string GenerateToken(string email, params string[] roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            // Choose a suitable algorithm based on your key size
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
            };

            // Add role claims if roles are provided
            if (roles != null && roles.Any())
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            var token = new JwtSecurityToken(
                issuer: "YourIssuerHere",
                audience: "YourAudienceHere",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1), 
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }


    }

}

