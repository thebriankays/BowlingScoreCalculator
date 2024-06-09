using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
                Frames = new List<FrameInput>();
                for (int i = 0; i < 10; i++)
                {
                    Frames.Add(new FrameInput());
                }
            }
        }
    }

    public class FrameInput
    {
        public int FirstRoll { get; set; }
        public int? SecondRoll { get; set; }
        public int? ThirdRoll { get; set; } 
    }
}
