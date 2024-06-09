using BowlingScoreCalculatorAPI.Commands;
using BowlingScoreCalculatorAPI.Models;
using BowlingScoreCalculatorAPI.Validators;
using FluentValidation.TestHelper;

namespace BowlingScoreCalculatorAPI.Tests.Validators
{
    public class CalculateScoreCommandValidatorTests
    {
        private readonly CalculateScoreCommandValidator _validator;

        public CalculateScoreCommandValidatorTests()
        {
            _validator = new CalculateScoreCommandValidator();
        }

        [Fact]
        public void Should_HaveValidationError_When_FramesCountIsNotTen()
        {
            var command = new CalculateScoreCommand([new Frame()]);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.Frames);
        }

        [Fact]
        public void Should_NotHaveValidationError_When_FramesCountIsTen()
        {
            var command = new CalculateScoreCommand(new List<Frame>(new Frame[10]));

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(c => c.Frames);
        }
    }
}
