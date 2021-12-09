using System;
using System.ComponentModel.DataAnnotations;
using EDMIDevices.Models.DAO;

namespace EDMIDevices.Models.Requests
{
    public class DeviceRequest
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "SerialNumber is required")]
        public string SerialNumber { get; set; }
        [Required(ErrorMessage = "FirmwareVersion is required")]
        public string FirmwareVersion { get; set; }
        public string DeviceType { get; set; }
        public string State { get; set; }
        public string Ip { get; set; }
        public int? Port { get; set; }
    }
}

