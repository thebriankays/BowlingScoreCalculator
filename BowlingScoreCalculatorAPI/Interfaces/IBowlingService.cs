using BowlingScoreCalculatorAPI.Models;

namespace BowlingScoreCalculatorAPI.Interfaces
{
    public interface IBowlingService
    {
        Task<int> CalculateScoreAsync(List<Frame> frames);
    }
}
