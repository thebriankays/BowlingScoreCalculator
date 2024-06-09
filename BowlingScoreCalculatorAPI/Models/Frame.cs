using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BowlingScoreCalculatorAPI.Models
{
    public class Frame
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Range(0, 10)]
        public int FirstRoll { get; set; }

        [Range(0, 10)]
        public int? SecondRoll { get; set; }

        [Range(0, 10)]
        public int? ThirdRoll { get; set; } // Only for the 10th frame

        public Guid? GameResultId { get; set; } 

        [NotMapped] 
        public GameResult? GameResult { get; set; } = null;
    }
}
