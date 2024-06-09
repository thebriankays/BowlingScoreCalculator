using BowlingScoreCalculatorAPI.Commands;
using BowlingScoreCalculatorAPI.Interfaces;
using MediatR;

namespace BowlingScoreCalculatorAPI.Handlers
{
    public class CalculateScoreCommandHandler(IBowlingService bowlingService, ILogger<CalculateScoreCommandHandler> logger) : IRequestHandler<CalculateScoreCommand, int>
    {
        private readonly IBowlingService _bowlingService = bowlingService;
        private readonly ILogger<CalculateScoreCommandHandler> _logger = logger;

        public async Task<int> Handle(CalculateScoreCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CalculateScoreCommand with {FrameCount} frames", request.Frames.Count);

            var score = await _bowlingService.CalculateScoreAsync(request.Frames);

            _logger.LogInformation("Score calculated: {Score}", score);
            return score;
        }
    }
}
