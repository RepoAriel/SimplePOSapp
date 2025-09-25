using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimplePOS.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Infrastructure.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser user); 
    }
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings settings;

        public JwtTokenGenerator(IOptions<JwtSettings> options)
        {
            this.settings = options.Value;
        }
        public string GenerateToken(ApplicationUser user)
        {
            if (string.IsNullOrWhiteSpace(settings.Key))
                throw new ArgumentException("JWT key no esta configurado de manera correcta");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("uid", user.Id)
            };

            var token = new JwtSecurityToken(
                    issuer: settings.Issuer,
                    audience: settings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(settings.ExpirationMinutes),
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
