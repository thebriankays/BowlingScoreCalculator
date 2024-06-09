using BowlingScoreCalculatorAPI.Interfaces;
using BowlingScoreCalculatorAPI.Models;
using BowlingScoreCalculatorAPI.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace BowlingScoreCalculatorAPI.Tests.Services
{
    public class BowlingServiceTests
    {
        private readonly Mock<IGameResultRepository> _gameResultRepositoryMock;
        private readonly Mock<ILogger<BowlingService>> _loggerMock;
        private readonly BowlingService _bowlingService;

        public BowlingServiceTests()
        {
            _gameResultRepositoryMock = new Mock<IGameResultRepository>();
            _loggerMock = new Mock<ILogger<BowlingService>>();

            _bowlingService = new BowlingService(
                _gameResultRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task CalculateScoreAsync_ShouldReturnCorrectScore()
        {
            // Arrange
            var frames = new List<Frame>
            {
                new Frame { FirstRoll = 10 },                                   // Strike
                new Frame { FirstRoll = 7, SecondRoll = 2 },                    // Open frame
                new Frame { FirstRoll = 3, SecondRoll = 5 },                    // Open frame
                new Frame { FirstRoll = 8, SecondRoll = 2 },                    // Spare
                new Frame { FirstRoll = 10 },                                   // Strike
                new Frame { FirstRoll = 10 },                                   // Strike
                new Frame { FirstRoll = 10 },                                   // Strike
                new Frame { FirstRoll = 6, SecondRoll = 4 },                    // Spare
                new Frame { FirstRoll = 7, SecondRoll = 3 },                    // Spare
                new Frame { FirstRoll = 10, SecondRoll = 10, ThirdRoll = 10 }   // Strike with bonus
            };

            _gameResultRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<GameResult>()))
                                     .Returns(Task.CompletedTask);

            // Act
            var score = await _bowlingService.CalculateScoreAsync(frames);

            // Assert
            score.Should().Be(199);
        }
    }
}
