using Confluent.Kafka;

using System.Text.Json;


namespace SportsEventsTracker.API.Services
{
    public class KafkaProducer<T>
    {
        private readonly IProducer<string,string> _producer;

        public KafkaProducer(string bootstrapServers)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers

            };
            _producer = new ProducerBuilder<string,string>(config).Build();
        }

        public async Task ProduceAsync(string topic, string key, T message){
        try{

            var messageValue = JsonSerializer.Serialize(message);

            var kafkaMessage = new Message<string,string>
            {
                Key=key,
                Value = messageValue

            };

            var deliveryResult = await _producer.ProduceAsync(topic,kafkaMessage);
            Console.WriteLine($"Message delivered to {deliveryResult.TopicPartitionOffset}");

            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Delivery failed: {ex.Error.Reason}");
            }
        }
    }
}