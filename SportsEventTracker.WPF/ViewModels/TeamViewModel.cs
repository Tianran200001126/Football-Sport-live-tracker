
using System.Collections.ObjectModel;
using System.Windows;
using SportsEventTracker.Models;
using SportsEventTracker.WPF.Services;
namespace SportsEventTracker.WPF.ViewModels
{
    public class TeamViewModel:ViewModelBase
    {
        private readonly ApiService _apiService;

        public ObservableCollection<Team> Teams { get; set; }

        private string _newTeamName;
        public string NewTeamName
        {
            get => _newTeamName;
            set
            {
                _newTeamName = value;
                OnPropertyChanged(nameof(NewTeamName));
                AddTeamCommand.RaiseCanExecuteChanged(); // Notify UI
            }
        }

        public string ErrorMessage { get; set; } // For binding error messages (optional)

        public RelayCommand AddTeamCommand { get; }

        public TeamViewModel()
        {
            _apiService = new ApiService();
            Teams = new ObservableCollection<Team>();
            LoadTeams();

            AddTeamCommand = new RelayCommand(
                async () => await AddTeamAsync(),
                () => !string.IsNullOrWhiteSpace(NewTeamName)
            );
        }

        private async void LoadTeams()
        {
            try
            {
                Teams.Clear();
                var fetchedTeams = await _apiService.GetTeamsAsync();
                foreach (var team in fetchedTeams)
                {
                    Teams.Add(team);
                }
            }
            catch (Exception ex)
            {
                // Handle server error during initialization
                MessageBox.Show($"Failed to load teams: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task AddTeamAsync()
        {
            if (string.IsNullOrWhiteSpace(NewTeamName))
                return;

            var newTeam = new Team {TeamName = NewTeamName };

            try
            {
                await _apiService.AddTeamAsync(newTeam); // API call
                Teams.Add(newTeam); // Add to local collection
                NewTeamName = string.Empty; // Clear input field
            }
               
            catch (Exception ex)
            {
                // Handle other errors
                MessageBox.Show($"An error occurred while adding the team: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
