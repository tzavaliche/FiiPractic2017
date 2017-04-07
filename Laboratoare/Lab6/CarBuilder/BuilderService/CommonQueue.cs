using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BuilderService
{
    public class CommonQueue
    {
        public IModel channel { get; set; }

        public void SendMessage(string msg)
        {
            if (channel != null)
            {
                var body = Encoding.UTF8.GetBytes(msg);

                var properties = channel.CreateBasicProperties();
                properties.SetPersistent(true);

                channel.BasicPublish(exchange: "", routingKey: "factory_line", basicProperties: properties, body: body);
            }
            else
            {
                throw new Exception("Please start the queue first!");
            }
        }

        public void StartQueue(EventHandler<BasicDeliverEventArgs> received)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "factory_line", durable: true, exclusive: false, autoDelete: false, arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += received;
                while (true)
                {
                    Thread.Sleep(1000);
                    channel.BasicConsume(queue: "factory_line", noAck: false, consumer: consumer);
                }
            }
        }
    }
}
