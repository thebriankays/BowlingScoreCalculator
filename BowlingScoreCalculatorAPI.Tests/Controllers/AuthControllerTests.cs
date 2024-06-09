using BowlingScoreCalculatorAPI.Controllers;
using BowlingScoreCalculatorAPI.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace BowlingScoreCalculatorAPI.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _configurationMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<AuthController>>();

            _controller = new AuthController(
                _userManagerMock.Object,
                _configurationMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserIsCreated()
        {
            // Arrange
            var model = new RegisterModel
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123!"
            };

            _userManagerMock.Setup(um => um.FindByNameAsync(model.Username))
                            .ReturnsAsync((ApplicationUser?)null);
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.Register(model) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
            result!.Value.Should().BeEquivalentTo(new { message = "User created successfully" });
        }

        [Fact]
        public async Task Register_ShouldReturnConflict_WhenUserAlreadyExists()
        {
            // Arrange
            var model = new RegisterModel
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123!"
            };

            _userManagerMock.Setup(um => um.FindByNameAsync(model.Username))
                            .ReturnsAsync(new ApplicationUser());

            // Act
            var result = await _controller.Register(model);

            // Assert
            result.Should().BeOfType<ConflictObjectResult>();
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WithToken_WhenCredentialsAreValid()
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

            _configurationMock.SetupGet(c => c["Jwt:Key"]).Returns("84160be5e469b063bb208395cb7af1d095d82fbc265fb8175eb21caacf0ba3c5");
            _configurationMock.SetupGet(c => c["Jwt:Issuer"]).Returns("testissuer");
            _configurationMock.SetupGet(c => c["Jwt:Audience"]).Returns("testaudience");

            // Act
            var result = await _controller.Login(model) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);

            // Use a strongly typed model for deserialization
            var valueJson = JsonSerializer.Serialize(result!.Value);
            var value = JsonSerializer.Deserialize<JsonElement>(valueJson);

            var token = value.GetProperty("token").GetString();
            var expiration = value.GetProperty("expiration").GetDateTime();

            token.Should().NotBeNullOrEmpty();
            expiration.Should().BeAfter(DateTime.Now);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var model = new LoginModel
            {
                Username = "testuser",
                Password = "InvalidPassword"
            };

            _userManagerMock.Setup(um => um.FindByNameAsync(model.Username))
                            .ReturnsAsync(new ApplicationUser());
            _userManagerMock.Setup(um => um.CheckPasswordAsync(It.IsAny<ApplicationUser>(), model.Password))
                            .ReturnsAsync(false);

            // Act
            var result = await _controller.Login(model);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }
    }
}
