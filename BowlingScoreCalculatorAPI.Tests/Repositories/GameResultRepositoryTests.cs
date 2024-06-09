using BowlingScoreCalculatorAPI.Data;
using BowlingScoreCalculatorAPI.Models;
using BowlingScoreCalculatorAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BowlingScoreCalculatorAPI.Tests.Repositories
{
    public class GameResultRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly GameResultRepository _repository;

        public GameResultRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BowlingDbTest")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new GameResultRepository(_context, Mock.Of<ILogger<GameResultRepository>>());

            // Seed the database
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var gameResult = new GameResult
            {
                Id = Guid.NewGuid(),
                Score = 200,
                Frames = new List<Frame>
                {
                    new Frame { FirstRoll = 10 }, // Strike
                    new Frame { FirstRoll = 9, SecondRoll = 1 } // Spare
                }
            };

            _context.GameResults.Add(gameResult);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnGameResult_WhenIdExists()
        {
            // Arrange
            var gameResultId = _context.GameResults.First().Id;

            // Act
            var result = await _repository.GetByIdAsync(gameResultId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(gameResultId, result.Id);
        }
    }
}
