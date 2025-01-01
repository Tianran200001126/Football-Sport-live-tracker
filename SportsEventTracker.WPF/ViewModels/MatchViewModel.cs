
using System.Collections.ObjectModel;
using SportsEventTracker.Models;
using SportsEventTracker.WPF.Services;

namespace SportsEventTracker.WPF.ViewModels
{
    public class MatchViewModel
    {
        private readonly ApiService _apiService;

        public ObservableCollection<GameMatch> Matches { get; set; }

        public MatchViewModel()
        {
            _apiService = new ApiService();
            Matches = new ObservableCollection<GameMatch>();
            LoadMatches();
        }

        private async void LoadMatches()
        {
            try
            {
                Matches.Clear();
                var fetchedMatches = await _apiService.GetMatchesAsync();
                foreach (var match in fetchedMatches)
                {
                    Matches.Add(match);
                }
            }
            catch (Exception ex)
            {
                // Handle errors, e.g., show a message to the user
                System.Windows.MessageBox.Show($"Failed to load matches: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
