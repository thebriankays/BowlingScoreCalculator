using BowlingScoreCalculatorAPI.Handlers;
using BowlingScoreCalculatorAPI.Interfaces;
using BowlingScoreCalculatorAPI.Models;
using BowlingScoreCalculatorAPI.Queries;
using Moq;

namespace BowlingScoreCalculatorAPI.Tests.Handlers
{
    public class GetGameResultByIdQueryHandlerTests
    {
        private readonly Mock<IGameResultRepository> _gameResultRepositoryMock;
        private readonly GetGameResultByIdQueryHandler _handler;

        public GetGameResultByIdQueryHandlerTests()
        {
            _gameResultRepositoryMock = new Mock<IGameResultRepository>();
            _handler = new GetGameResultByIdQueryHandler(_gameResultRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnGameResult_WhenGameResultExists()
        {
            // Arrange
            var gameResult = new GameResult { Id = Guid.NewGuid(), Score = 100 };
            var query = new GetGameResultByIdQuery { GameResultId = gameResult.Id };

            _gameResultRepositoryMock.Setup(repo => repo.GetByIdAsync(gameResult.Id))
                                     .ReturnsAsync(gameResult);

            // Act
            var result = await _handler.HandleAsync(query);

            // Assert
            Assert.Equal(gameResult, result);
        }
    }
}
