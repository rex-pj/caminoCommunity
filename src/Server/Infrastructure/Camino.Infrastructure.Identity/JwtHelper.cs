using Camino.Infrastructure.Identity.Options;
using Camino.Infrastructure.Identity.Core;
using Camino.Infrastructure.Identity.Interfaces;
using Camino.Shared.Constants;
using Camino.Shared.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Camino.Infrastructure.Identity
{
    public class JwtHelper : IJwtHelper
    {
        private readonly JwtConfigOptions _jwtConfigOptions;
        public JwtHelper(IOptions<JwtConfigOptions> jwtConfigOptions)
        {
            _jwtConfigOptions = jwtConfigOptions.Value;
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(_jwtConfigOptions.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(HttpHeaders.UserIdentityClaimKey, user.UserIdentityId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(_jwtConfigOptions.HourExpires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        public JwtSecurityToken GetSecurityToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = Encoding.ASCII.GetBytes(_jwtConfigOptions.SecretKey);
                var claimPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = _jwtConfigOptions.Issuer,
                    ValidAudience = _jwtConfigOptions.Audience
                }, out SecurityToken securityToken);

                var jwtSecurityToken = securityToken as JwtSecurityToken;
                return jwtSecurityToken;
            }
            catch (SecurityTokenExpiredException ex)
            {
                throw new CaminoAuthenticationException(ex.Message, ErrorMessages.TokenExpiredException);
            }
        }
    }
}
