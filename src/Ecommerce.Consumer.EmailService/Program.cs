using System;
using Ecommerce.Core;

namespace Consumer.EmailService
{
    class Program
    {
        public static void Main(string[] args)
        {
            using (var consumer = new KafkaConsumer<Email>("ECOMMERCE_SEND_EMAIL"))
            {
                consumer.OnMessage((value) =>
                {
                    Console.WriteLine($"Enviando o e-mail '{value.Body}' para '{value.To}'.");
                });
            }
        }
    }
}