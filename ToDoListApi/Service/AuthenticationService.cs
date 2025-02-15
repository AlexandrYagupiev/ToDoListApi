using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToDoListAPI.Data;
using ToDoListAPI.Models;

namespace ToDoListAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> Authenticate(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
            if (existingUser == null)
            {
                return null;
            }

            var secretKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                new Claim(ClaimTypes.Email, existingUser.Email),
            };

            var jwtToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public ClaimsPrincipal GetUserClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            return principal;
        }
    }
}