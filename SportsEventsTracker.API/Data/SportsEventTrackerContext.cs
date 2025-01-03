using Microsoft.EntityFrameworkCore;
using SportsEventTracker.Models;

namespace SportsEventTracker.API.Data
{
    public class SportsEventTrackerContext : DbContext
    {
        public SportsEventTrackerContext(DbContextOptions<SportsEventTrackerContext> options) : base(options) { }

        public DbSet<Team> Teams { get; set; }
        public DbSet<GameMatch> Matches { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<GameMatch>(entity => {entity.ToTable(t => t.HasCheckConstraint("CK_Match_TeamNamesNotEqual", "[TeamAName] != [TeamBName]"));});
        modelBuilder.Entity<Team>()
            .HasKey(t => t.TeamName); // Primary key for Team

        modelBuilder.Entity<GameMatch>()
            .HasKey(t => t.MatchID); // Primary key for Team    

        modelBuilder.Entity<Team>()
            .HasMany(t => t.MatchesAsTeamA)
            .WithOne(m => m.TeamA)
            .HasForeignKey(m => m.TeamAName)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete for TeamA

        modelBuilder.Entity<Team>()
            .HasMany(t => t.MatchesAsTeamB)
            .WithOne(m => m.TeamB)
            .HasForeignKey(m => m.TeamBName)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete for TeamB
    }


    }

}