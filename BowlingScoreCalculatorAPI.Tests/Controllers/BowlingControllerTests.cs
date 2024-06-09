using Xunit;
using Moq;
using MediatR;
using Microsoft.Extensions.Logging;
using BowlingScoreCalculatorAPI.Controllers;
using BowlingScoreCalculatorAPI.Commands;
using BowlingScoreCalculatorAPI.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                new FrameInput(10, null, null), // Strike
                new FrameInput(7, 2, null),     // Open frame
                new FrameInput(3, 5, null),     // Open frame
                new FrameInput(8, 2, null),     // Spare
                new FrameInput(10, null, null), // Strike
                new FrameInput(10, null, null), // Strike
                new FrameInput(10, null, null), // Strike
                new FrameInput(6, 4, null),     // Spare
                new FrameInput(7, 3, null),     // Spare
                new FrameInput(10, 10, 10)      // Strike with bonus
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
                new FrameInput(10, null, null) // Only 1 frame instead of 10
            };

            // Act
            var result = await _controller.CalculateScore(frameInputs);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
