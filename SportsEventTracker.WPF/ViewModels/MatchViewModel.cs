using System.Collections.ObjectModel;
using System.Windows;
using SportsEventTracker.Models;
using SportsEventTracker.Services;
using SportsEventTracker.WPF.Services;
namespace SportsEventTracker.WPF.ViewModels
{
    public class MatchViewModel : ViewModelBase
    {
        private readonly ApiService _apiService;
        private readonly KafkaConsumerService _kafkaConsumerService;
        private CancellationTokenSource _cancellationTokenSource;

        private ObservableCollection<GameMatch> _matches;

        public ObservableCollection<GameMatch> Matches
        {
            get => _matches;
            set
            {
                _matches = value;
                OnPropertyChanged(nameof(Matches));
            }
        }

        public MatchViewModel()
        {
            _apiService = new ApiService();
            Matches = new ObservableCollection<GameMatch>();
            _kafkaConsumerService = KafkaConsumerService.GetInstance("localhost:9092", "update-score");
            _cancellationTokenSource = new CancellationTokenSource();

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            try
            {
                StartKafkaConsumer();
                await LoadMatchesAsync();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadMatchesAsync()
        {
            try
            {
                Matches.Clear();
                var fetchedMatches = await _apiService.GetMatchesAsync();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var match in fetchedMatches)
                    {
                        Matches.Add(match);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load matches: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StartKafkaConsumer()
        {
            Task.Run(async () =>
            {
                try
                {
                    await _kafkaConsumerService.StartConsumingAsync(Matches, _cancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Kafka consumer error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

    }
}
