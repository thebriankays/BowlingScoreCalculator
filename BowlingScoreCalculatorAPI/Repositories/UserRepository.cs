using BowlingScoreCalculatorAPI.Data;
using BowlingScoreCalculatorAPI.Interfaces;
using BowlingScoreCalculatorAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BowlingScoreCalculatorAPI.Repositories
{
    public class UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger) : Repository<ApplicationUser>(context), IUserRepository
    {
        private new readonly ApplicationDbContext _context = context;
        private readonly ILogger<UserRepository> _logger = logger;

        public new async Task<ApplicationUser?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Getting ApplicationUser by Id: {Id}", id);
            return await _context.Users.FindAsync(id.ToString());
        }

        public new async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            _logger.LogInformation("Getting all ApplicationUsers");
            return await _context.Users.ToListAsync();
        }

        public new async Task<IEnumerable<ApplicationUser>> FindAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            _logger.LogInformation("Finding ApplicationUsers with predicate");
            return await _context.Users.Where(predicate).ToListAsync();
        }

        public async Task<ApplicationUser?> GetByUsernameAsync(string username)
        {
            _logger.LogInformation("Getting ApplicationUser by Username: {Username}", username);
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
