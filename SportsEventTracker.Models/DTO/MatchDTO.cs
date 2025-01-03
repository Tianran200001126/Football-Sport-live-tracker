

namespace SportsEventsTracker.DTO
{
  
  public class GameMatchDto
  {
    public string TeamAName{get;set;}
    public string TeamBName{get;set;}

    public int ScoreA{get;set;}=0;
    public int ScoreB{get;set;}=0;
  }

      public class MatchResponseDto
    {
        public Guid MatchID { get; set; }
        public string TeamAName { get; set; }
        public string TeamBName { get; set; }
        public int ScoreA { get; set; }
        public int ScoreB { get; set; }
    }
     public class UpdateScoreDto
    {
        /// <summary>
        /// The unique ID of the match.
        /// </summary>
        public Guid MatchID { get; set; }

        /// <summary>
        /// The name of the team whose score is to be updated.
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// The new score to be assigned to the specified team.
        /// </summary>
        public int NewScore { get; set; }
    }

}