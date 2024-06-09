using BowlingScoreCalculatorAPI.Queries;
using FluentAssertions;

namespace BowlingScoreCalculatorAPI.Tests.Queries
{
    public class GetGameResultByIdQueryTests
    {
        [Fact]
        public void Constructor_ShouldSetGameResultIdProperty()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var query = new GetGameResultByIdQuery { GameResultId = id };

            // Assert
            query.GameResultId.Should().Be(id);
        }
    }
}
