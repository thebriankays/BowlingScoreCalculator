using BowlingScoreCalculatorAPI.Interfaces;
using BowlingScoreCalculatorAPI.Models;

namespace BowlingScoreCalculatorAPI.Services
{
    public class BowlingService(IGameResultRepository gameResultRepository, ILogger<BowlingService> logger) : IBowlingService
    {
        private readonly IGameResultRepository _gameResultRepository = gameResultRepository;
        private readonly ILogger<BowlingService> _logger = logger;

        public async Task<int> CalculateScoreAsync(List<Frame> frames)
        {
            _logger.LogInformation("Calculating score for game with {FrameCount} frames", frames.Count);

            var game = new Game { Frames = frames };
            var score = game.CalculateScore();

            _logger.LogInformation("Calculated score: {Score}", score);

            var gameResult = new GameResult
            {
                Frames = frames,
                Score = score
            };

            await _gameResultRepository.AddAsync(gameResult);
            return score;
        }
    }
}
