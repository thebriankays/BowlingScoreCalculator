using BowlingScoreCalculatorAPI.Commands;
using BowlingScoreCalculatorAPI.Handlers;
using BowlingScoreCalculatorAPI.Interfaces;
using BowlingScoreCalculatorAPI.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace BowlingScoreCalculatorAPI.Tests.Handlers
{
    public class CalculateScoreCommandHandlerTests
    {
        private readonly Mock<IBowlingService> _bowlingServiceMock;
        private readonly Mock<ILogger<CalculateScoreCommandHandler>> _loggerMock;
        private readonly CalculateScoreCommandHandler _handler;

        public CalculateScoreCommandHandlerTests()
        {
            _bowlingServiceMock = new Mock<IBowlingService>();
            _loggerMock = new Mock<ILogger<CalculateScoreCommandHandler>>();
            _handler = new CalculateScoreCommandHandler(_bowlingServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnCorrectScore()
        {
            // Arrange
            var frames = new List<Frame>
            {
                new() { FirstRoll = 10 },
                new() { FirstRoll = 7, SecondRoll = 3 }
            };
            var command = new CalculateScoreCommand(frames);
            _bowlingServiceMock.Setup(s => s.CalculateScoreAsync(frames)).ReturnsAsync(20);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.Equal(20, result);
        }
    }
}
