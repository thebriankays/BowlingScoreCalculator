using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BowlingScoreCalculatorFrontend.Pages
{
    public class BowlingFormModel : PageModel
    {
        [BindProperty]
        public List<FrameInput> Frames { get; set; } = [];

        [BindProperty]
        public int? Score { get; set; }

        public BowlingFormModel()
        {
            InitializeFrames();
        }

        public void OnGet()
        {
            InitializeFrames();
        }

        private void InitializeFrames()
        {
            if (Frames == null || Frames.Count != 10)
            {
                Frames = [];
                for (int i = 0; i < 10; i++)
                {
                    Frames.Add(new FrameInput());
                }
            }
        }
    }

    public class FrameInput
    {
        [Range(0, 10, ErrorMessage = "First roll must be between 0 and 10.")]
        public int FirstRoll { get; set; }

        [Range(0, 10, ErrorMessage = "Second roll must be between 0 and 10.")]
        public int? SecondRoll { get; set; }

        [Range(0, 10, ErrorMessage = "Third roll must be between 0 and 10.")]
        public int? ThirdRoll { get; set; }
    }
}
