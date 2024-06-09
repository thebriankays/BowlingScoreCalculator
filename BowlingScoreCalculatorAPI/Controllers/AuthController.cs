using BowlingScoreCalculatorAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BowlingScoreCalculatorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            _logger.LogInformation("Register attempt for user {Username}", model.Username);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for user registration");
                return BadRequest(ModelState);
            }

            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                _logger.LogWarning("User {Username} already exists", model.Username);
                return Conflict(new { message = "User already exists" });
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("User creation failed for {Username}: {Errors}", model.Username, string.Join(", ", result.Errors.Select(e => $"{e.Code}: {e.Description}")));
                return BadRequest(new { message = "User creation failed", errors = result.Errors });
            }

            _logger.LogInformation("User {Username} successfully created", model.Username);
            return Ok(new { message = "User created successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            _logger.LogInformation("Login attempt for user {Username}", model.Username);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for user login");
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                _logger.LogWarning("Invalid login attempt for user {Username}", model.Username);
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var authClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                _logger.LogError("JWT key is not configured");
                return StatusCode(500, "Internal server error");
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            _logger.LogInformation("User {Username} successfully logged in", model.Username);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}