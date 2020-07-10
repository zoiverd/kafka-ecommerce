using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Ecommerce.Core
{
    public class KafkaProducer<T> : IDisposable
    {
        private readonly IProducer<string, T> producer;

        public KafkaProducer()
        {
            if (typeof(T) == typeof(string))
            {
                throw new InvalidProgramException("Can't produce messages as string");
            }
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            producer = new ProducerBuilder<string, T>(config)
                .SetValueSerializer(new JsonSerializer<T>())
                .Build();

            // If serializers are not specified, default serializers from
            // `Confluent.Kafka.Serializers` will be automatically used where
            // available. Note: by default strings are encoded as UTF8.
        }

        public async Task Send(string topic, string key, T value)
        {
            try
            {
                var dr = await producer.ProduceAsync(topic, new Message<string, T> { Key = key, Value = value });
                Console.WriteLine($"   => Delivered NEW_ORDER '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            }
            catch (ProduceException<string, T> e)
            {
                Console.WriteLine($"   => Delivery failed: {e.Error.Reason}");
            }
        }

        public void Dispose()
        {
            producer.Dispose();
        }
    }
}