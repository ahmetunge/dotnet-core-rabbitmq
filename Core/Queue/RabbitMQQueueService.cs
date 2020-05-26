using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Core.Queue
{
    public class RabbitMQQueueService : IQueueService
    {
        private readonly ILogger<RabbitMQQueueService> _logger;

        public RabbitMQQueueService(
            ILogger<RabbitMQQueueService> logger)
        {
            _logger = logger;
        }
        public void AddQueue<T>(IEnumerable<T> queueDataModels, string queueName) where T : class, new()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost"
                };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                                        queue: queueName,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null
                                        );

                    foreach (var queueDataModel in queueDataModels)
                    {

                        string strBody = JsonConvert.SerializeObject(queueDataModel);
                        var body = Encoding.UTF8.GetBytes(strBody);
                        channel.BasicPublish(exchange: "",
                                             routingKey: queueName,
                                             body: body);
                        _logger.LogInformation($"{strBody} Message added queue");
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Message could not be added to the queue");
            }
        }

        public void ReadFromQueue()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                _logger.LogInformation("Consumer is started");
                channel.QueueDeclare(queue: "Helloworld",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var data = Encoding.UTF8.GetString(body);
                    //MessageConsumer message = JsonConvert.DeserializeObject<MessageConsumer>(data);
                    _logger.LogInformation("[x] Received data : " + data);
                    //  Console.WriteLine("[x] Received data : " + data);
                };
                channel.BasicConsume(queue: "Helloworld",
                                     autoAck: true,
                                     consumer: consumer);
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}