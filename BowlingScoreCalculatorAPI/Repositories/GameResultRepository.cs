using BowlingScoreCalculatorAPI.Data;
using BowlingScoreCalculatorAPI.Interfaces;
using BowlingScoreCalculatorAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BowlingScoreCalculatorAPI.Repositories
{
    public class GameResultRepository(ApplicationDbContext context, ILogger<GameResultRepository> logger) : Repository<GameResult>(context), IGameResultRepository
    {
        private new readonly ApplicationDbContext _context = context;
        private readonly ILogger<GameResultRepository> _logger = logger;

        public new async Task<GameResult?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Getting GameResult by Id: {Id}", id);
            return await _context.GameResults.Include(gr => gr.Frames)
                                             .FirstOrDefaultAsync(gr => gr.Id == id);
        }

        public new async Task<IEnumerable<GameResult>> GetAllAsync()
        {
            _logger.LogInformation("Getting all GameResults");
            return await _context.GameResults.Include(gr => gr.Frames).ToListAsync();
        }

        public new async Task<IEnumerable<GameResult>> FindAsync(Expression<Func<GameResult, bool>> predicate)
        {
            _logger.LogInformation("Finding GameResults with predicate");
            return await _context.GameResults.Include(gr => gr.Frames)
                                             .Where(predicate)
                                             .ToListAsync();
        }

        public new async Task AddAsync(GameResult gameResult)
        {
            _logger.LogInformation("Adding new GameResult");
            await _context.GameResults.AddAsync(gameResult);
            await _context.SaveChangesAsync();
        }

        public new async Task AddRangeAsync(IEnumerable<GameResult> gameResults)
        {
            _logger.LogInformation("Adding new GameResults");
            await _context.GameResults.AddRangeAsync(gameResults);
            await _context.SaveChangesAsync();
        }

        public new async Task RemoveAsync(GameResult gameResult)
        {
            _logger.LogInformation("Removing GameResult");
            _context.GameResults.Remove(gameResult);
            await _context.SaveChangesAsync();
        }

        public new async Task RemoveRangeAsync(IEnumerable<GameResult> gameResults)
        {
            _logger.LogInformation("Removing GameResults");
            _context.GameResults.RemoveRange(gameResults);
            await _context.SaveChangesAsync();
        }

        public new async Task UpdateAsync(GameResult gameResult)
        {
            _logger.LogInformation("Updating GameResult");
            _context.GameResults.Update(gameResult);
            await _context.SaveChangesAsync();
        }
    }
}
