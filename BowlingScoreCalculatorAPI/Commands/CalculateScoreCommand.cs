using BowlingScoreCalculatorAPI.Models;
using MediatR;

namespace BowlingScoreCalculatorAPI.Commands
{
    public class CalculateScoreCommand : IRequest<int>
    {
        public List<Frame> Frames { get; set; }

        public CalculateScoreCommand(List<Frame> frames)
        {
            Frames = frames ?? [];
        }

        public CalculateScoreCommand()
        {
            Frames = [];
        }
    }
}
