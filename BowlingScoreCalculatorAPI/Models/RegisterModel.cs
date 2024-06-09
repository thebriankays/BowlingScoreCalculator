using System.ComponentModel.DataAnnotations;

namespace BowlingScoreCalculatorAPI.Models
{
    public class RegisterModel : LoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
    }
}
