using BowlingScoreCalculatorAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace BowlingScoreCalculatorAPI.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterModel model);
        Task<string?> LoginUserAsync(LoginModel model);
    }
}
