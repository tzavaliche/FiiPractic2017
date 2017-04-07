using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarBuilder
{
    public class ReadWriteQueue
    {
        public static void SendBuildRequest(string msg)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "factory_line", durable: true, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(msg);

                var properties = channel.CreateBasicProperties();
                properties.SetPersistent(true);

                channel.BasicPublish(exchange: "", routingKey: "factory_line", basicProperties: properties, body: body);
            }
        }

        public static string ReadBuildStatus(string id)
        {
            var response = string.Empty;

            var factory = new ConnectionFactory() { HostName = "localhost" };
           
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "factory_line", durable: true, exclusive: false, autoDelete: false, arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var msgSplit = message.Split('|');
                    if(msgSplit[0] == "B" && msgSplit.Length == 4 && msgSplit[1] == id)
                    {
                        response = message;
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                };

                //read queue until we find the message corresponding to the car Id
                while (response == string.Empty)
                {
                    Thread.Sleep(500);
                    channel.BasicConsume(queue: "factory_line", noAck: false, consumer: consumer);
                }
                return response;
            }
        }
    }
}
