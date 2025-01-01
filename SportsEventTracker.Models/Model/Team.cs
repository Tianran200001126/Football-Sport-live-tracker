
using System.Text.Json.Serialization;
namespace SportsEventTracker.Models
{
    public class Team
    {
        public string TeamName { get; set; } // Primary key

        // Navigation properties for matches

        public ICollection<GameMatch> MatchesAsTeamA { get; set; } = new List<GameMatch>();
 
        public ICollection<GameMatch> MatchesAsTeamB { get; set; } = new List<GameMatch>();
    }
}
