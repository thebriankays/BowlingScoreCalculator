using BowlingScoreCalculatorAPI.Models;
using Xunit;
using System.Collections.Generic;

namespace BowlingScoreCalculatorAPI.Tests.Models
{
    public class GameTests
    {
        [Fact]
        public void CalculateScore_ShouldReturnZero_ForNoPinsKnockedDown()
        {
            var game = new Game
            {
                Frames =
                [
                    new Frame { FirstRoll = 0, SecondRoll = 0 },
                    new Frame { FirstRoll = 0, SecondRoll = 0 },
                    new Frame { FirstRoll = 0, SecondRoll = 0 },
                    new Frame { FirstRoll = 0, SecondRoll = 0 },
                    new Frame { FirstRoll = 0, SecondRoll = 0 },
                    new Frame { FirstRoll = 0, SecondRoll = 0 },
                    new Frame { FirstRoll = 0, SecondRoll = 0 },
                    new Frame { FirstRoll = 0, SecondRoll = 0 },
                    new Frame { FirstRoll = 0, SecondRoll = 0 },
                    new Frame { FirstRoll = 0, SecondRoll = 0, ThirdRoll = 0 }
                ]
            };

            var score = game.CalculateScore();

            Assert.Equal(0, score);
        }

        [Fact]
        public void CalculateScore_ShouldReturnCorrectScore_ForAllSpares()
        {
            var game = new Game
            {
                Frames =
                [
                    new Frame { FirstRoll = 5, SecondRoll = 5 },
                    new Frame { FirstRoll = 5, SecondRoll = 5 },
                    new Frame { FirstRoll = 5, SecondRoll = 5 },
                    new Frame { FirstRoll = 5, SecondRoll = 5 },
                    new Frame { FirstRoll = 5, SecondRoll = 5 },
                    new Frame { FirstRoll = 5, SecondRoll = 5 },
                    new Frame { FirstRoll = 5, SecondRoll = 5 },
                    new Frame { FirstRoll = 5, SecondRoll = 5 },
                    new Frame { FirstRoll = 5, SecondRoll = 5 },
                    new Frame { FirstRoll = 5, SecondRoll = 5, ThirdRoll = 5 }
                ]
            };

            var score = game.CalculateScore();

            Assert.Equal(150, score);
        }

        [Fact]
        public void CalculateScore_ShouldReturnCorrectScore_ForAllStrikes()
        {
            var game = new Game
            {
                Frames =
                [
                    new Frame { FirstRoll = 10 },
                    new Frame { FirstRoll = 10 },
                    new Frame { FirstRoll = 10 },
                    new Frame { FirstRoll = 10 },
                    new Frame { FirstRoll = 10 },
                    new Frame { FirstRoll = 10 },
                    new Frame { FirstRoll = 10 },
                    new Frame { FirstRoll = 10 },
                    new Frame { FirstRoll = 10 },
                    new Frame { FirstRoll = 10, SecondRoll = 10, ThirdRoll = 10 }
                ]
            };

            var score = game.CalculateScore();

            Assert.Equal(300, score);
        }

        [Fact]
        public void CalculateScore_ShouldReturnCorrectScore_ForMixedGame()
        {
            var game = new Game
            {
                Frames =
                [
                    new Frame { FirstRoll = 10 },
                    new Frame { FirstRoll = 7, SecondRoll = 3 },
                    new Frame { FirstRoll = 9, SecondRoll = 0 },
                    new Frame { FirstRoll = 10 },
                    new Frame { FirstRoll = 0, SecondRoll = 8 },
                    new Frame { FirstRoll = 8, SecondRoll = 2 },
                    new Frame { FirstRoll = 0, SecondRoll = 6 },
                    new Frame { FirstRoll = 10 },
                    new Frame { FirstRoll = 10 },
                    new Frame { FirstRoll = 10, SecondRoll = 8, ThirdRoll = 1 }
                ]
            };

            var score = game.CalculateScore();

            Assert.Equal(167, score);
        }

        [Fact]
        public void CalculateScore_ShouldHandleEmptyFrames()
        {
            var game = new Game
            {
                Frames = []
            };

            var score = game.CalculateScore();

            Assert.Equal(0, score);
        }

        [Fact]
        public void CalculateScore_ShouldHandlePartiallyFilledGame()
        {
            var game = new Game
            {
                Frames =
                [
                    new Frame { FirstRoll = 10 },
                    new Frame { FirstRoll = 7, SecondRoll = 3 }
                ]
            };

            var score = game.CalculateScore();

            Assert.Equal(30, score);
        }

        [Fact]
        public void CalculateScore_ShouldHandleOpenFrames()
        {
            var game = new Game
            {
                Frames =
                [
                    new Frame { FirstRoll = 3, SecondRoll = 4 },
                    new Frame { FirstRoll = 2, SecondRoll = 6 }
                ]
            };

            var score = game.CalculateScore();

            Assert.Equal(15, score);
        }

        [Fact]
        public void CalculateScore_ShouldHandleZeroFrames()
        {
            var game = new Game
            {
                Frames =
                [
                    new Frame { FirstRoll = 0, SecondRoll = 0 },
                    new Frame { FirstRoll = 0, SecondRoll = 0 }
                ]
            };

            var score = game.CalculateScore();

            Assert.Equal(0, score);
        }

        [Fact]
        public void CalculateScore_ShouldHandlePartialStrikeBonus()
        {
            var game = new Game
            {
                Frames =
                [
                    new Frame { FirstRoll = 10 },
                    new Frame { FirstRoll = 5, SecondRoll = 3 }
                ]
            };

            var score = game.CalculateScore();

            Assert.Equal(26, score);
        }

        [Fact]
        public void CalculateScore_ShouldHandleNoSpareBonus()
        {
            var game = new Game
            {
                Frames =
                [
                    new Frame { FirstRoll = 5, SecondRoll = 5 },
                    new Frame { FirstRoll = 0, SecondRoll = 0 }
                ]
            };

            var score = game.CalculateScore();

            Assert.Equal(10, score);
        }

        [Fact]
        public void CalculateScore_ShouldHandle10thFrameSpare()
        {
            var game = new Game
            {
                Frames =
                [
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 5, SecondRoll = 5, ThirdRoll = 5 } 
                ]
            };

            var score = game.CalculateScore();

            Assert.Equal(33, score);
        }

        [Fact]
        public void CalculateScore_ShouldHandle10thFrameStrike()
        {
            var game = new Game
            {
                Frames =
                [
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 1, SecondRoll = 1 },
                    new Frame { FirstRoll = 10, SecondRoll = 10, ThirdRoll = 10 }
                ]
            };

            var score = game.CalculateScore();

            Assert.Equal(48, score);
        }
    }
}
