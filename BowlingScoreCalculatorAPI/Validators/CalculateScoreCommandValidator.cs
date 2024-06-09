using BowlingScoreCalculatorAPI.Commands;
using BowlingScoreCalculatorAPI.Models;
using FluentValidation;

namespace BowlingScoreCalculatorAPI.Validators
{
    public class CalculateScoreCommandValidator : AbstractValidator<CalculateScoreCommand>
    {
        public CalculateScoreCommandValidator()
        {
            RuleFor(command => command.Frames)
                .NotNull()
                .Must(frames => frames.Count == 10)
                .WithMessage("Exactly 10 frames are required.");

            RuleForEach(command => command.Frames).SetValidator(new FrameValidator());
        }
    }

    public class FrameValidator : AbstractValidator<Frame>
    {
        public FrameValidator()
        {
            RuleFor(frame => frame.FirstRoll).InclusiveBetween(0, 10);
            RuleFor(frame => frame.SecondRoll)
                .InclusiveBetween(0, 10)
                .When(frame => frame.FirstRoll < 10); // Only validate if it's not a strike
            RuleFor(frame => frame.ThirdRoll)
                .InclusiveBetween(0, 10)
                .When(frame => frame.FirstRoll == 10 || frame.SecondRoll == 10); // Only validate for the 10th frame
        }
    }
}
