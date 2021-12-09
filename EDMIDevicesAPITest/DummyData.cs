using System;
using EDMIDevices.Models.Config;
using EDMIDevices.Models.DAO;
using EDMIDevices.Models.Requests;

namespace EDMIDevicesAPITest
{
    public class DummyData
    {
        public static DeviceRequest CreateDeviceRequestDummy(string serial)
        {
            return new DeviceRequest()
            {
                DeviceType = EDeviceType.Gateway.ToString(),
                SerialNumber = serial,
                FirmwareVersion = "Version",
                State = "Ok",
                Ip = "172.0.0.1",
                Port = 80
            };
        }

        public static string GenerateRandomId()
        {
            return new Guid().ToString();
        }

        public static DeviceRequest CreateDeviceRequestDummyNotValid()
        {
            return new DeviceRequest()
            {
                DeviceType = EDeviceType.Gateway.ToString(),
                SerialNumber = "Serial",
                State = "Ok",
                Ip = "172.0.0.1",
                Port = 80
            };
        }

        public static DeviceRequest CreateDeviceRequestDummyNotValidDeviceType()
        {
            return new DeviceRequest()
            {
                DeviceType = "Test",
                SerialNumber = "Serial",
                FirmwareVersion = "Version",
                State = "Ok",
                Ip = "172.0.0.1",
                Port = 80
            };
        }

        public static DatabaseSettings CreateDatabaseSettingsTest()
        {
            return new DatabaseSettings {
                CollectionName = "DevicesTest",
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "EDMIDevices"
            };
        }
    }
}

