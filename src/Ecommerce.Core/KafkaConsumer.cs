using System;
using System.Reflection;
using System.Threading;
using Confluent.Kafka;

namespace Ecommerce.Core
{
    public class KafkaConsumer<T> : IDisposable
    {
        private readonly string topic;
        private readonly string groupId;
        private readonly IConsumer<string, T> consumer;

        public KafkaConsumer(String topic)
        {
            this.topic = topic;
            this.groupId = Assembly.GetEntryAssembly().GetName().Name;

            var conf = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = "localhost:9092",
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest,
                ClientId = groupId,
            };

            var builder = new ConsumerBuilder<string, T>(conf);

            if (typeof(T) != typeof(string))
            {
                builder.SetValueDeserializer(new JsonSerializer<T>());
            }

            consumer = builder.Build();
        }

        public void OnMessage(Action<T> receiveMessage)
        {
            consumer.Subscribe(topic);
            Console.WriteLine($"Starting Group: {groupId}");

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            try
            {
                while (true)
                {
                    try
                    {
                        var cr = consumer.Consume(cts.Token);
                        receiveMessage?.Invoke(cr.Message.Value);
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }

        public void Dispose()
        {
            consumer.Close();
        }
    }
}
