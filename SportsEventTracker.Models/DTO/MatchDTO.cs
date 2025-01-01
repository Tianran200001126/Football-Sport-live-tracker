

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

}