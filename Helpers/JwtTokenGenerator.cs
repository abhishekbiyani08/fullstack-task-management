// using System;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.Extensions.Configuration;
// using Microsoft.IdentityModel.Tokens;
// using TaskManagementApi.Models;

// namespace TaskManagementApi.Helpers
// {
//     public class JwtTokenGenerator
//     {
//         private readonly IConfiguration _config;

//         public JwtTokenGenerator(IConfiguration config)
//         {
//             _config = config;
//         }

//         public string GenerateToken(User user)
//         {
//             var claims = new[]
//             {
//                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//                 new Claim(ClaimTypes.Name, user.Username!)
//             };

//             // Get the key once and validate it properly
//             var keyString = _config.GetSection("AppSettings:Token").Value;
//             if (string.IsNullOrEmpty(keyString) || keyString.Length < 32)
//             {
//                 throw new InvalidOperationException("JWT token key must be at least 32 characters long");
//             }

//             var keyBytes = Encoding.UTF8.GetBytes(keyString);
//             var key = new SymmetricSecurityKey(keyBytes);

//             var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
//             var tokenDescriptor = new SecurityTokenDescriptor
//             {
//                 Subject = new ClaimsIdentity(claims),
//                 Expires = DateTime.UtcNow.AddDays(7),
//                 SigningCredentials = creds,
//             };

//             var tokenHandler = new JwtSecurityTokenHandler();
//             var token = tokenHandler.CreateToken(tokenDescriptor);
//             return tokenHandler.WriteToken(token);
//         }
//     }
// }

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskManagementApi.Models;

namespace TaskManagementApi.Helpers
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _config;

        public JwtTokenGenerator(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            // Add userId as a claim using ClaimTypes.NameIdentifier
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // userId
                new Claim(ClaimTypes.Name, user.Username!) // username
            };

            // Get the key from configuration
            var keyString = _config.GetSection("AppSettings:Token").Value;
            if (string.IsNullOrEmpty(keyString) || keyString.Length < 32)
            {
                throw new InvalidOperationException("JWT token key must be at least 32 characters long");
            }

            var keyBytes = Encoding.UTF8.GetBytes(keyString);
            var key = new SymmetricSecurityKey(keyBytes);

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // Create the token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Token expiration
                SigningCredentials = creds,
            };

            // Generate the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}