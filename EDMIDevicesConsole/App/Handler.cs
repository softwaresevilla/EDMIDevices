using System;
using System.Text.Json;
using System.Threading.Tasks;
using EDMIDevices.Models.Requests;
using EDMIDevicesAPI.Models.Requests;
using RabbitMQ.Client;

namespace EDMIDevicesConsole.App
{
    public class Handler
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;

        public Handler()
        {
            _factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest"};
            _connection = _factory.CreateConnection();
        }

        public async Task<bool> AddDeviceHandler(DeviceRequest device)
        {
            DeviceQueueRequest request = new DeviceQueueRequest
            {
                Task = "create",
                Data = JsonSerializer.Serialize(device)
            };
            return await sendJob(request);
        }

        public async Task<bool> UpdateDeviceHandler(DeviceRequest device)
        {
            DeviceQueueRequest request = new DeviceQueueRequest
            {
                Task = "update",
                Data = JsonSerializer.Serialize(device)
            };
            return await sendJob(request);
        }

        public async Task<bool> RemoveDeviceHandler(string id)
        {
            DeviceQueueRequest request = new DeviceQueueRequest
            {
                Task = "delete",
                Data = id
            };
            return await sendJob(request);
        }

        private async Task<bool> sendJob(DeviceQueueRequest task)
        {
            try
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "devices", durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var json = JsonSerializer.Serialize(task);
                    var body = System.Text.Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: "devices", basicProperties: null, body: body);
                }

                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"An error occurs sending job to the queue: {e.Message}");
            }

            return await Task.FromResult(false);
        }
    }
}

