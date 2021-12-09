using System;
using EDMIDevices.Models.DAO;

namespace EDMIDevices.Models.DTO
{
    public class DeviceDTO
    {
        public string Id { get; set; }
        public string SerialNumber { get; set; }
        public string FirmwareVersion { get; set; }
        public string DeviceType { get; set; }
        public string State { get; set; }
        public string Ip { get; set; }
        public int? Port { get; set; }
    }
}

