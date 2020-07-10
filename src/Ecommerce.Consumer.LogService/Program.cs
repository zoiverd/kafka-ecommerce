using System;
using Ecommerce.Core;

namespace Consumer.LogService
{
    class Program
    {
        public static void Main(string[] args)
        {
            using (var consumer = new KafkaConsumer<string>("^ECOMMERCE.*"))
            {
                consumer.OnMessage((value) =>
                {
                    Console.WriteLine($"Logando: '{value}'.");
                });
            }
        }
    }
}
