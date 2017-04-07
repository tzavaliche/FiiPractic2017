using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ChassisService
{
    public class Worker
    {
        public void ProcessQueue()
        {
            var queue = new CommonQueue();
            EventHandler<BasicDeliverEventArgs> received = (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                //process message
                var msgSplit = message.Split('|');
                if (msgSplit[0].Equals("C") && msgSplit.Length == 3)
                {
                    Console.WriteLine("Chassis received: " + message);
                    //Received request for chassis
                    //Process and respond with OK
                    var responseMsg = message + "|OK";
                    queue.SendMessage(responseMsg);
                    Console.WriteLine("Chassis sent: " + responseMsg);
                    queue.channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                else
                {
                    //if the message is not for us, release it
                    queue.channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
                }
            };

            queue.StartQueue(received);
        }
    }
}
