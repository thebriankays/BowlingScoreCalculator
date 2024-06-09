using BowlingScoreCalculatorAPI.Data;
using BowlingScoreCalculatorAPI.Models;
using BowlingScoreCalculatorAPI.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BowlingScoreCalculatorAPI.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly UserRepository _repository;
        private readonly ApplicationDbContext _context;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                          .UseInMemoryDatabase(Guid.NewGuid().ToString())
                          .Options;

            _context = new ApplicationDbContext(options);
            _repository = new UserRepository(_context, Mock.Of<ILogger<UserRepository>>());
        }

        [Fact]
        public async Task GetByUsernameAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "testuser" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByUsernameAsync("testuser");

            // Assert
            result.Should().NotBeNull();
            result!.UserName.Should().Be("testuser");
        }

        [Fact]
        public async Task GetByUsernameAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Act
            var result = await _repository.GetByUsernameAsync("nonexistentuser");

            // Assert
            result.Should().BeNull();
        }
    }
}
