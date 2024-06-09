namespace BowlingScoreCalculatorAPI.Models
{
    public class Game
    {
        public List<Frame> Frames { get; set; } = [];

        public int CalculateScore()
        {
            int score = 0;

            for (int i = 0; i < Frames.Count; i++)
            {
                if (i < 9) // First 9 frames
                {
                    if (IsStrike(Frames[i]))
                    {
                        score += 10 + StrikeBonus(i);
                    }
                    else if (IsSpare(Frames[i]))
                    {
                        score += 10 + SpareBonus(i);
                    }
                    else
                    {
                        score += Frames[i].FirstRoll + Frames[i].SecondRoll.GetValueOrDefault();
                    }
                }
                else // 10th frame
                {
                    score += Frames[i].FirstRoll + Frames[i].SecondRoll.GetValueOrDefault() + Frames[i].ThirdRoll.GetValueOrDefault();
                }
            }

            return score;
        }

        private bool IsStrike(Frame frame) => frame.FirstRoll == 10;

        private bool IsSpare(Frame frame) => frame.FirstRoll + frame.SecondRoll.GetValueOrDefault() == 10;

        private int StrikeBonus(int frameIndex)
        {
            if (frameIndex + 1 >= Frames.Count) return 0;

            var nextFrame = Frames[frameIndex + 1];
            if (IsStrike(nextFrame))
            {
                if (frameIndex + 2 < Frames.Count)
                {
                    return 10 + Frames[frameIndex + 2].FirstRoll;
                }
                return 10 + nextFrame.SecondRoll.GetValueOrDefault();
            }

            return nextFrame.FirstRoll + nextFrame.SecondRoll.GetValueOrDefault();
        }

        private int SpareBonus(int frameIndex)
        {
            if (frameIndex + 1 >= Frames.Count) return 0;

            return Frames[frameIndex + 1].FirstRoll;
        }
    }
}
