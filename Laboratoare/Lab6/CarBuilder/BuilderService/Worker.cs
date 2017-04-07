using RabbitMQ.Client.Events;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace BuilderService
{
    public class Worker
    {

        private void PutBuildCarToProducer(string id, string carInfo)
        {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri("http://localhost:58355/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent content;
                HttpResponseMessage response = client.PutAsync("api/producer/" + id, new StringContent(carInfo, Encoding.UTF8, "application/json")).Result;
                response.EnsureSuccessStatusCode();
            }
        }

        public void ProcessQueue()
        {
            var queue = new CommonQueue();
            EventHandler<BasicDeliverEventArgs> received = (model, ea) =>
            {

                bool isProcessed = false;
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);


                //process message
                var msgSplit = message.Split('|');
                switch (msgSplit[0])
                {
                    //Builder Message Processing
                    case "B":
                        //Message Split Length: 3 - REQUEST and 4 - RESPONSE
                        if (msgSplit.Length == 3)
                        {
                            //Process the request

                            //Check if CarCode(Chassis, Engine, Interior, Wheels) is valid (has 4 characters)
                            if (msgSplit[2].Length == 4)
                            {
                                Console.WriteLine("Builder processing Build Car request: " + message);
                                //Send message to ChassisService
                                var chassisRequestMsg = "C|" + msgSplit[1] + "|" + msgSplit[2][0]; //e.g.: C|100|1 (C|Id|ChassisCode)
                                queue.SendMessage(chassisRequestMsg);
                                Console.WriteLine("Builder sent Chassis request: " + chassisRequestMsg);


                                //Finish processing by acknowledging message as processed
                                isProcessed = true;
                                queue.channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                            }
                        }
                        break;
                    case "C":
                        if (msgSplit.Length == 4)
                        {
                            //Received response from Chassis
                            Console.WriteLine("Builder processing Chassis response: " + message);

                            PutBuildCarToProducer(msgSplit[1], "\"" + msgSplit[2] + "|C \"");

                            isProcessed = true;
                            queue.channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                        break;
                    default:
                        break;
                }

                if (!isProcessed)
                {
                    //if the message is not processed by us, release it
                    queue.channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
                }
            };

            queue.StartQueue(received);
        }
    }
}
