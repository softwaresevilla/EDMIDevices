using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EDMIDevices.Models.DAO
{
    public enum EDeviceType
    {
        ElectricMeter = 0,
        WaterMeter = 1,
        Gateway = 2
    }

    public class BaseDevice
    {
        [BsonId]
        public string Id { get; set; }
        public string SerialNumber { get; set; }
        public string FirmwareVersion { get; set; }
        public EDeviceType DeviceType { get; set; }
    }
}