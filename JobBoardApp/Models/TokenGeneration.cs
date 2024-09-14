using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobBoardApp.Models
{
    /// <summary>
    /// Handles JWT token generation based on configuration settings and claims.
    /// </summary>
    public class TokenGeneration
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenGeneration"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to access application settings.</param>
        public TokenGeneration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Generates a JWT token based on the provided claims.
        /// </summary>
        /// <param name="claims">The claims to include in the token.</param>
        /// <returns>The generated JWT token as a string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the JWT secret, issuer, or audience is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when the token expiry time is not a valid number.</exception>
        public string GenerateToken(IEnumerable<Claim> claims)
        {
            ErrorMessages Messages = new ErrorMessages();
            var jwtSecret = configuration["JWTKey:Secret"];
            var validIssuer = configuration["JWTKey:ValidIssuer"];
            var validAudience = configuration["JWTKey:ValidAudience"];
            var tokenExpiryInHours = configuration["JWTKey:TokenExpiryTimeInHour"];

            if (string.IsNullOrEmpty(jwtSecret))
                throw new ArgumentNullException(nameof(jwtSecret), Messages.JWTSecretNull);

            if (string.IsNullOrEmpty(validIssuer))
                throw new ArgumentNullException(nameof(validIssuer), Messages.JWTIssuerEmpty);

            if (string.IsNullOrEmpty(validAudience))
                throw new ArgumentNullException(nameof(validAudience), Messages.JWTAudienceEmpty);

            if (string.IsNullOrEmpty(tokenExpiryInHours))
                throw new ArgumentNullException(nameof(jwtSecret), Messages.JWTTokenExpireEmpty);

            if (!double.TryParse(tokenExpiryInHours, out double tokenExpiryInHoursDouble))
                throw new ArgumentException(Messages.JWTTokenExpireDoubleType);

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = validIssuer,
                Audience = validAudience,
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.AddHours(tokenExpiryInHoursDouble),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
