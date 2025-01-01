using System;

using System.Text.Json.Serialization;
namespace SportsEventTracker.Models
{
    public class GameMatch
    {
        public Guid MatchID { get; set; } // Primary key
        public DateTime MatchDate { get; set; } // Date of the match

        // Foreign keys
        public string TeamAName { get; set; } // Foreign key to Team.TeamName
        public string TeamBName { get; set; } // Foreign key to Team.TeamName

        public Team TeamA { get; set; } // Team A details

        public Team TeamB { get; set; } // Team B details

        // Scores
        public int ScoreA { get; set; } // Score for Team A
        public int ScoreB { get; set; } // Score for Team B
    }
}
