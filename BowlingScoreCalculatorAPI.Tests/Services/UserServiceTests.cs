using BowlingScoreCalculatorAPI.Models;
using BowlingScoreCalculatorAPI.Services;
using BowlingScoreCalculatorAPI.Tests.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.IdentityModel.Tokens.Jwt;

namespace BowlingScoreCalculatorAPI.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly TestLoggerProvider _loggerProvider;
        private readonly ILogger<UserService> _logger;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.SetupGet(c => c["Jwt:Key"]).Returns("84160be5e469b063bb208395cb7af1d095d82fbc265fb8175eb21caacf0ba3c5");
            _configurationMock.SetupGet(c => c["Jwt:Issuer"]).Returns("testissuer");
            _configurationMock.SetupGet(c => c["Jwt:Audience"]).Returns("testaudience");

            _loggerProvider = new TestLoggerProvider();
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(_loggerProvider);
            _logger = loggerFactory.CreateLogger<UserService>();

            _userService = new UserService(_userManagerMock.Object, _configurationMock.Object, _logger);
        }

        [Fact]
        public async Task Register_ShouldLogError_WhenCreationFails()
        {
            // Arrange
            var model = new RegisterModel
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123!"
            };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
                            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "TestCode", Description = "Test error" }));

            // Act
            var result = await _userService.RegisterUserAsync(model);

            // Assert
            result.Succeeded.Should().BeFalse();
            _loggerProvider.Contains(LogLevel.Error, "User registration failed for testuser").Should().BeTrue();
        }

        [Fact]
        public async Task Register_ShouldCreateUser_WhenModelIsValid()
        {
            // Arrange
            var model = new RegisterModel
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123!"
            };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.RegisterUserAsync(model);

            // Assert
            result.Succeeded.Should().BeTrue();
            _loggerProvider.Contains(LogLevel.Information, "User testuser registered successfully").Should().BeTrue();
        }

        [Fact]
        public async Task Login_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            var model = new LoginModel
            {
                Username = "nonexistentuser",
                Password = "Password123!"
            };

            _userManagerMock.Setup(um => um.FindByNameAsync(model.Username))
                            .ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await _userService.LoginUserAsync(model);

            // Assert
            result.Should().BeNull();
            _loggerProvider.Contains(LogLevel.Warning, "User nonexistentuser not found").Should().BeTrue();
        }

        [Fact]
        public async Task Login_ShouldReturnNull_WhenPasswordIsInvalid()
        {
            // Arrange
            var model = new LoginModel
            {
                Username = "testuser",
                Password = "WrongPassword"
            };

            var user = new ApplicationUser { UserName = model.Username };
            _userManagerMock.Setup(um => um.FindByNameAsync(model.Username))
                            .ReturnsAsync(user);
            _userManagerMock.Setup(um => um.CheckPasswordAsync(user, model.Password))
                            .ReturnsAsync(false);

            // Act
            var result = await _userService.LoginUserAsync(model);

            // Assert
            result.Should().BeNull();
            _loggerProvider.Contains(LogLevel.Warning, "Invalid password for user testuser").Should().BeTrue();
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var model = new LoginModel
            {
                Username = "testuser",
                Password = "Password123!"
            };

            var user = new ApplicationUser { UserName = model.Username };
            _userManagerMock.Setup(um => um.FindByNameAsync(model.Username))
                            .ReturnsAsync(user);
            _userManagerMock.Setup(um => um.CheckPasswordAsync(user, model.Password))
                            .ReturnsAsync(true);

            // Act
            var result = await _userService.LoginUserAsync(model);

            // Assert
            result.Should().NotBeNull();
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(result!) as JwtSecurityToken;
            token.Should().NotBeNull();
            token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value.Should().Be(model.Username);
            _loggerProvider.Contains(LogLevel.Information, "User testuser successfully authenticated").Should().BeTrue();
        }

        [Fact]
        public async Task Login_ShouldThrowException_WhenJwtKeyIsNotConfigured()
        {
            // Arrange
            _configurationMock.SetupGet(c => c["Jwt:Key"]).Returns((string)null!); // Simulate missing key

            var model = new LoginModel
            {
                Username = "testuser",
                Password = "Password123!"
            };

            var user = new ApplicationUser { UserName = model.Username };
            _userManagerMock.Setup(um => um.FindByNameAsync(model.Username))
                            .ReturnsAsync(user);
            _userManagerMock.Setup(um => um.CheckPasswordAsync(user, model.Password))
                            .ReturnsAsync(true);

            // Act
            Func<Task> act = () => _userService.LoginUserAsync(model);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("JWT key is not configured");
        }
    }
}
