using System;

namespace EDMIDevices.Models.DAO
{
    public class Device : BaseDevice
    {
        public string State { get; set; }
        public string Ip { get; set; }
        public int? Port { get; set; }
    }
}

