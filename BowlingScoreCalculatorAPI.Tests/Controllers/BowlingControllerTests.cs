using BowlingScoreCalculatorAPI.Commands;
using BowlingScoreCalculatorAPI.Controllers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BowlingScoreCalculatorAPI.Tests.Controllers
{
    public class BowlingControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<BowlingController>> _loggerMock;
        private readonly BowlingController _controller;

        public BowlingControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<BowlingController>>();

            _controller = new BowlingController(
                _mediatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task CalculateScore_ShouldReturnCorrectScore()
        {
            // Arrange
            var frameInputs = new List<FrameInput>
            {
                new(10, null, null), // Strike
                new(7, 2, null),     // Open frame
                new(3, 5, null),     // Open frame
                new(8, 2, null),     // Spare
                new(10, null, null), // Strike
                new(10, null, null), // Strike
                new(10, null, null), // Strike
                new(6, 4, null),     // Spare
                new(7, 3, null),     // Spare
                new(10, 10, 10)      // Strike with bonus
            };

            var expectedScore = 193;

            _mediatorMock.Setup(m => m.Send(It.IsAny<CalculateScoreCommand>(), default))
                         .ReturnsAsync(expectedScore);

            // Act
            var result = await _controller.CalculateScore(frameInputs) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
            result!.Value.Should().BeEquivalentTo(new { score = expectedScore });
        }

        [Fact]
        public async Task CalculateScore_ShouldReturnBadRequestForInvalidFrames()
        {
            // Arrange
            var frameInputs = new List<FrameInput>
            {
                new(10, null, null) // Only 1 frame instead of 10
            };

            // Act
            var result = await _controller.CalculateScore(frameInputs);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
