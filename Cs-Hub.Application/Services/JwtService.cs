using Cs_Hub.Application.DTO;
using Cs_Hub.Application.ServiceContracts;
using Cs_Hub.Application.Settings;
using Cs_Hub.Domain.entities.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace Cs_Hub.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public LoginResponseDTO CreateJwtToken(ApplicationUser user, IEnumerable<string> roles)
        {
            DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings.EXPIRATION_MINUTES));
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!)
            };
            claims.AddRange(roleClaims);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenGenerator = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: expiration,
                signingCredentials: signingCredentials
                );
            
            var tokenHandler = new JwtSecurityTokenHandler();
            string token = tokenHandler.WriteToken(tokenGenerator);

            return new LoginResponseDTO
            {
                Token = token,
                Expiration = expiration,
                Email = user.Email,
                Role = roles
            };
        }
    }
}
