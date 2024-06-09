using BowlingScoreCalculatorAPI.Commands;
using BowlingScoreCalculatorAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BowlingScoreCalculatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BowlingController(IMediator mediator, ILogger<BowlingController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<BowlingController> _logger = logger;

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateScore([FromBody] List<FrameInput> frames)
        {
            _logger.LogInformation("Score calculation request received with {FrameCount} frames", frames.Count);

            if (frames == null || frames.Count != 10)
            {
                _logger.LogWarning("Invalid frame count: {FrameCount}", frames?.Count ?? 0);
                return BadRequest("Invalid input. Please provide exactly 10 frames.");
            }

            try
            {
                var frameModels = new List<Frame>();
                foreach (var frame in frames)
                {
                    frameModels.Add(new Frame
                    {
                        FirstRoll = frame.FirstRoll,
                        SecondRoll = frame.SecondRoll,
                        ThirdRoll = frame.ThirdRoll
                    });
                }

                var command = new CalculateScoreCommand(frameModels);
                var totalScore = await _mediator.Send(command);

                _logger.LogInformation("Score calculation completed successfully");

                return Ok(new { score = totalScore });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calculating score");
                return StatusCode(500, "An error occurred while calculating the score.");
            }
        }
    }
    public record FrameInput(int FirstRoll, int? SecondRoll, int? ThirdRoll);
}