using Microsoft.AspNetCore.Identity;
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
        Task<string> GenerateToken(ApplicationUser user); 
    }
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings settings;
        private readonly UserManager<ApplicationUser> userManager;

        public JwtTokenGenerator(IOptions<JwtSettings> options, UserManager<ApplicationUser> userManager)
        {
            this.settings = options.Value;
            this.userManager = userManager;
        }
        public async Task<string> GenerateToken(ApplicationUser user)
        {
            if (string.IsNullOrWhiteSpace(settings.Key))
                throw new ArgumentException("JWT key no esta configurado de manera correcta");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("uid", user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

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
