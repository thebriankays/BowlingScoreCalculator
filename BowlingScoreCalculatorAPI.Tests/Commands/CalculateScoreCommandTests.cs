using BowlingScoreCalculatorAPI.Commands;
using BowlingScoreCalculatorAPI.Models;
using FluentAssertions;

namespace BowlingScoreCalculatorAPI.Tests.Commands
{
    public class CalculateScoreCommandTests
    {
        [Fact]
        public void Constructor_ShouldSetFramesProperty()
        {
            // Arrange
            var frames = new List<Frame>
            {
                new() { FirstRoll = 10 },
                new() { FirstRoll = 7, SecondRoll = 3 }
            };

            // Act
            var command = new CalculateScoreCommand(frames);

            // Assert
            command.Frames.Should().BeEquivalentTo(frames);
        }

        [Fact]
        public void Constructor_ShouldSetEmptyFramesProperty_WhenNoFramesProvided()
        {
            // Act
            var command = new CalculateScoreCommand();

            // Assert
            command.Frames.Should().BeEmpty();
        }
    }
}
