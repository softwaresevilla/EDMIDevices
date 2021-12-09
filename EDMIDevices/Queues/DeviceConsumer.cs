using EDMIDevices.Models.DAO;
using EDMIDevices.Models.Requests;
using EDMIDevices.Repositories;
using EDMIDevices.Storage;
using EDMIDevicesAPI.Models.Config;
using EDMIDevicesAPI.Models.Requests;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EDMIDevicesAPI.Queues
{
    class DeviceConsumer : BackgroundService
    {

        private readonly IDeviceRepository _repository;
        private readonly QueueSettings _queueSettings;
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public DeviceConsumer(IDeviceRepository repository, QueueSettings queueSettings)
        {
            _repository = repository;
            _queueSettings = queueSettings;

            _factory = new ConnectionFactory()
            {
                HostName = _queueSettings.Host,
                Port = _queueSettings.Port,
                UserName = _queueSettings.User,
                Password = _queueSettings.Password
            };

            _connection = _factory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: _queueSettings.QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var task = JsonSerializer.Deserialize<DeviceQueueRequest>(Encoding.UTF8.GetString(body));

                    switch (task.Task)
                    {
                        case "create":
                            _repository.Add(JsonSerializer.Deserialize<DeviceRequest>(task.Data));
                            break;
                        case "update":
                            var data = JsonSerializer.Deserialize<DeviceRequest>(task.Data);
                            _repository.Update(data);
                            break;
                        case "delete":
                            _repository.Delete(task.Data);
                            break;
                        default:
                            Console.WriteLine($"Unknow Task {task}, received");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error with queue operation {e.Message}");
                }
            };
                
            _channel.BasicConsume(queue: _queueSettings.QueueName,
                                  autoAck: true,
                                  consumer: consumer);
            return Task.CompletedTask;
        }
    }
}
