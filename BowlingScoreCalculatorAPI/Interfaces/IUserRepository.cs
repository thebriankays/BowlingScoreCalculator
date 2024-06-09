using BowlingScoreCalculatorAPI.Models;

namespace BowlingScoreCalculatorAPI.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetByUsernameAsync(string username);
    }
}
