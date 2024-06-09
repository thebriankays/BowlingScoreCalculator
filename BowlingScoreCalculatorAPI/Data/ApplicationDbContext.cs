using BowlingScoreCalculatorAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BowlingScoreCalculatorAPI.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<GameResult> GameResults { get; set; }
        public DbSet<Frame> Frames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Frame>()
                .HasOne(f => f.GameResult)
                .WithMany(gr => gr.Frames)
                .HasForeignKey(f => f.GameResultId);
        }
    }
}
