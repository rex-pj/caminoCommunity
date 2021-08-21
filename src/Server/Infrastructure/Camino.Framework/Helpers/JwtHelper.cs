using Camino.Core.Contracts.Helpers;
using Camino.Core.Contracts.IdentityManager;
using Camino.Core.Domain.Identities;
using Camino.Infrastructure.Commons.Constants;
using Camino.Shared.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Camino.Framework.Helpers
{
    public class JwtHelper : IJwtHelper
    {
        private readonly JwtConfigOptions _jwtConfigOptions;
        private readonly IUserManager<ApplicationUser> _userManager;
        public JwtHelper(IOptions<JwtConfigOptions> jwtConfigOptions, IUserManager<ApplicationUser> userManager)
        {
            _jwtConfigOptions = jwtConfigOptions.Value;
            _userManager = userManager;
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(_jwtConfigOptions.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(HttpHeaderContants.USER_IDENTITY_ID_CLAIM_KEY, user.UserIdentityId.ToString()),
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

        public async Task<ClaimsIdentity> ValidateTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(_jwtConfigOptions.SecretKey);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
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

            if (securityToken != null)
            {
                var jwtToken = securityToken as JwtSecurityToken;
                var userIdentityId = jwtToken.Claims.First(x => x.Type == HttpHeaderContants.USER_IDENTITY_ID_CLAIM_KEY).Value;
                var userId = await _userManager.DecryptUserIdAsync(userIdentityId);
                var claimIdentity = new ClaimsIdentity(jwtToken.Claims, JwtBearerDefaults.AuthenticationScheme);
                claimIdentity.AddClaim(new Claim(HttpHeaderContants.USER_ID_CLAIM_KEY, userId.ToString()));

                return claimIdentity;
            }

            return new ClaimsIdentity();
        }
    }
}
