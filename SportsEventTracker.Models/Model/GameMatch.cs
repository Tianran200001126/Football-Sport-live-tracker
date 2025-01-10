using System;
using System.ComponentModel;

namespace SportsEventTracker.Models
{
    public class GameMatch :INotifyPropertyChanged
    {
        private int _scoreA;
        private int _scoreB;
        public Guid MatchID { get; set; } // Primary key
        public DateTime MatchDate { get; set; } // Date of the match

        // Foreign keys
        public string TeamAName { get; set; } // Foreign key to Team.TeamName
        public string TeamBName { get; set; } // Foreign key to Team.TeamName

        public Team TeamA { get; set; } // Team A details

        public Team TeamB { get; set; } // Team B details

            
        public int ScoreA
        {
            get => _scoreA;
            set
            {
                if (_scoreA != value)
                {
                    _scoreA = value;
                    OnPropertyChanged(nameof(ScoreA));
                }
            }
        }


        public int ScoreB
        {
            get => _scoreB;
            set
            {
                if (_scoreB != value)
                {
                    _scoreB = value;
                    OnPropertyChanged(nameof(ScoreB));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
