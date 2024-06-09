using BowlingScoreCalculatorAPI.Interfaces;
using BowlingScoreCalculatorAPI.Models;
using BowlingScoreCalculatorAPI.Queries;

namespace BowlingScoreCalculatorAPI.Handlers
{
    public class GetGameResultByIdQueryHandler(IGameResultRepository gameResultRepository) : IQueryHandler<GetGameResultByIdQuery, GameResult?>
    {
        private readonly IGameResultRepository _gameResultRepository = gameResultRepository;

        public async Task<GameResult?> HandleAsync(GetGameResultByIdQuery query)
        {
            return await _gameResultRepository.GetByIdAsync(query.GameResultId);
        }
    }
}
