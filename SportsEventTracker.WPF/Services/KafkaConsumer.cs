using Confluent.Kafka;
using System.Collections.ObjectModel;
using System.Text.Json;
using SportsEventTracker.Models;
using System.Windows;
using SportsEventsTracker.DTO;

namespace SportsEventTracker.Services
{
    public class KafkaConsumerService
    {
        private readonly string _bootstrapServers;
        private readonly string _topic;

        public KafkaConsumerService(string bootstrapServers, string topic)
        {
            _bootstrapServers = bootstrapServers;
            _topic = topic;
        }

        public async Task StartConsumingAsync(ObservableCollection<GameMatch> matches, CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = "wpf-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

            LogInfo("Waiting for messages...");

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    LogInfo("Start consuming messages...");
                    var result = consumer.Consume(cancellationToken);
                    if (result != null)
                    {
                        try
                        {
                            var updateScore = JsonSerializer.Deserialize<UpdateScoreDto>(result.Message.Value);
                            UpdateUI(matches, updateScore);
                        }
                        catch (JsonException ex)
                        {
                            LogError($"Failed to deserialize message: {ex.Message}");
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo("Consumer stopped gracefully.");
            }
            catch (Exception ex)
            {
                LogError($"Unexpected error: {ex.Message}");
            }
            finally
            {
                consumer.Close();
            }
        }

private void UpdateUI(ObservableCollection<GameMatch> matches, UpdateScoreDto updateScore)
{
    // Ensure thread-safe operation
    Application.Current.Dispatcher.Invoke(() =>
    {
        // Find the index of the match to be updated
        var index = matches.IndexOf(matches.FirstOrDefault(m => m.MatchID == updateScore.MatchID));
        
        if (index >= 0)
        {
            LogInfo($"Replacing match for: {matches[index].TeamAName} vs {matches[index].TeamBName} score {updateScore.NewScore}");
            
            // Replace the existing match with an updated one
            matches[index] = new GameMatch
            {
                MatchID = matches[index].MatchID,
                TeamAName = matches[index].TeamAName,
                TeamBName = matches[index].TeamBName,
                ScoreA = updateScore.TeamName == matches[index].TeamAName ? updateScore.NewScore : matches[index].ScoreA,
                ScoreB = updateScore.TeamName == matches[index].TeamBName ? updateScore.NewScore : matches[index].ScoreB
            };
        }
        else
        {
            LogInfo($"Match with ID {updateScore.MatchID} not found.");
        }
    });
}


        private void LogInfo(string message)
        {
            Console.WriteLine($"INFO: {message}");
        }

        private void LogError(string message)
        {
            Console.WriteLine($"ERROR: {message}");
        }
    }
}
