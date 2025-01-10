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
    private static KafkaConsumerService _instance;
    private static readonly object _lock = new();

    private readonly string _bootstrapServers;
    private readonly string _topic;
    private IConsumer<Ignore, string> _consumer;

    private KafkaConsumerService(string bootstrapServers, string topic)
    {
        _bootstrapServers = bootstrapServers;
        _topic = topic;

        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = "wpf-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Latest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
    }

    public static KafkaConsumerService GetInstance(string bootstrapServers, string topic)
    {
        lock (_lock)
        {
            return _instance ??= new KafkaConsumerService(bootstrapServers, topic);
        }
    }

    public async Task StartConsumingAsync(ObservableCollection<GameMatch> matches, CancellationToken cancellationToken)
    {
        try
        {
            _consumer.Subscribe(_topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(cancellationToken);
                if (result?.Message?.Value != null)
                {
                    var update = JsonSerializer.Deserialize<UpdateScoreDto>(result.Message.Value);
                    UpdateUI(matches, update);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Graceful shutdown
        }
        finally
        {
            _consumer.Close();
        }
    }


     private void LogInfo(string message)
        {
            Console.WriteLine($"INFO: {message}");
        }

  
private void UpdateUI(ObservableCollection<GameMatch> matches, UpdateScoreDto updateScore)
{
    Application.Current.Dispatcher.BeginInvoke(() =>
    {
        var match = matches.FirstOrDefault(m => m.MatchID == updateScore.MatchID);
        if (match != null)
        {
            LogInfo($"Updating match for: {match.TeamAName} vs {match.TeamBName} with new score {updateScore.NewScore}");
            if (updateScore.TeamName == match.TeamAName)
                match.ScoreA = updateScore.NewScore;
            else if (updateScore.TeamName == match.TeamBName)
                match.ScoreB = updateScore.NewScore;
        }
        else
        {
            LogInfo($"Match with ID {updateScore.MatchID} not found.");
        }
    });
}

}
}

