using System;
namespace EDMIDevicesAPI.Models.Config
{
    public class QueueSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string QueueName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}

