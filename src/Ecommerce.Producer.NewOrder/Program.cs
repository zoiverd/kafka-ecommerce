using System;
using System.Threading.Tasks;
using Ecommerce.Core;

namespace Producer.NewOrder
{

    class Program
    {
        static async Task Main(string[] args)
        {
            using (var producerOrder = new KafkaProducer<Order>())
            using (var producerEmail = new KafkaProducer<Email>())
            {
                var rnd = new Random();
                for (var i = 0; i < 10; i++)
                {
                    var order = new Order(Guid.NewGuid().ToString("N"), $"Pedido_{i}", rnd.Next(1, 5000));
                    var email = new Email($"{order.UserId}@teste.com", $"{order.OrderId} processado");

                    await producerOrder.Send("ECOMMERCE_NEW_ORDER", order.UserId, order);
                    await producerEmail.Send("ECOMMERCE_SEND_EMAIL", order.UserId, email);
                }
            }
        }
    }

}