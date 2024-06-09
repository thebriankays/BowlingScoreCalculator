using BowlingScoreCalculatorAPI.Interfaces;
using BowlingScoreCalculatorAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BowlingScoreCalculatorAPI.Services
{
    public class UserService(UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger<UserService> logger) : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<UserService> _logger = logger;

        public async Task<string?> LoginUserAsync(LoginModel model)
        {
            _logger.LogInformation("Login attempt for user {Username}", model.Username);

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                _logger.LogWarning("User {Username} not found", model.Username);
                return null;
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                _logger.LogWarning("Invalid password for user {Username}", model.Username);
                return null;
            }

            _logger.LogInformation("User {Username} successfully authenticated", model.Username);

            return GenerateJwtToken(user);
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
        {
            _logger.LogInformation("Registering user {Username}", model.Username);

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {Username} registered successfully", model.Username);
            }
            else
            {
                _logger.LogError("User registration failed for {Username}: {Errors}", model.Username, string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}")));
            }

            return result;
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var authClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured");
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured");
            var jwtAudience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT audience is not configured");

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogInformation("JWT generated for user {Username}", user.UserName);

            return tokenString;
        }
    }
}
