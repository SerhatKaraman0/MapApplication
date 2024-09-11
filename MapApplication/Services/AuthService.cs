using System;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MapApplication.Data;
using MapApplication.Interfaces;
using MapApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MapApplication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        // Token blacklist - In-memory for demonstration
        private static readonly ConcurrentDictionary<string, bool> _blacklistedTokens = new();

        public AuthService(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        // Modified to include custom claims (UserId, UserName)
        public async Task<string> GenerateToken(UsersDb user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserEmail),     // Email
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
                new Claim("UserId", user.UserId.ToString()),                // Custom claim: UserId
                new Claim("UserName", user.UserName)                        // Custom claim: UserName
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Authenticate user and return token with custom claims
        public async Task<IActionResult> AuthenticateUser(string email, string password)
        {
            // Replace with actual user fetching logic
            var user = await _userService.GetUserByEmailAndPassword(email, password);
            if (user == null)
            {
                return new UnauthorizedResult(); // Return 401 Unauthorized if credentials are invalid
            }

            var token = await GenerateToken(user);
            var response = new
            {
                Success = true,
                Token = token,
                UserId = user.UserId,   // Return the UserId as part of the response
                UserName = user.UserName
            };

            return new OkObjectResult(response); // Return 200 OK with the token and user details
        }

        // Logout logic
        public void LogoutUser(string token)
        {
            // Add the token to the blacklist
            _blacklistedTokens[token] = true;
        }

        // Check if the token is blacklisted
        public bool IsTokenBlacklisted(string token)
        {
            return _blacklistedTokens.ContainsKey(token);
        }
    }
}
