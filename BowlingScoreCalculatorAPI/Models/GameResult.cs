using System.ComponentModel.DataAnnotations;

namespace BowlingScoreCalculatorAPI.Models
{
    public class GameResult
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<Frame> Frames { get; set; } = [];

        public int Score { get; set; }
    }
}
