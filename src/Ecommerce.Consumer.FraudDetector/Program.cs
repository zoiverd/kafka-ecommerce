using System;
using Ecommerce.Core;

namespace Consumer.FraudDetector
{
    class Program
    {
        public static void Main(string[] args)
        {
            using (var consumer = new KafkaConsumer<string>("ECOMMERCE_SEND_EMAIL"))
            {
                consumer.OnMessage((value) =>
                {
                    Console.WriteLine($"Processando a fraude: '{value}'.");
                });
            }
        }
    }
}
