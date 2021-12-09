using System;
using System.Collections.Generic;
using System.Linq;
using EDMIDevices.Exceptions;
using EDMIDevices.Models.DAO;
using EDMIDevices.Models.DTO;
using EDMIDevices.Models.Requests;
using EDMIDevices.Storage;

namespace EDMIDevices.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly IStorage<Device> _storage;

        public DeviceRepository(IStorage<Device> storage)
        {
            _storage = storage;
        }

        public DeviceDTO Add(DeviceRequest deviceRequest)
        {
            if (string.IsNullOrEmpty(deviceRequest.SerialNumber)) throw new NullReferenceException($"{nameof(deviceRequest.SerialNumber)} is mandatory");
            if (string.IsNullOrEmpty(deviceRequest.FirmwareVersion)) throw new NullReferenceException($"{nameof(deviceRequest.FirmwareVersion)} is mandatory");
            if (!Enum.TryParse<EDeviceType>(deviceRequest.DeviceType, out var enumValue)) throw new UnsupportedDeviceTypeException($"The device type {deviceRequest.DeviceType} does not exists");
            if (IsDuplicated(deviceRequest.SerialNumber)) throw new DuplicatedDeviceException($"The device you try to add is already created");
            var deviceType = (EDeviceType) Enum.Parse(typeof(EDeviceType), deviceRequest.DeviceType, true);
            var id = Guid.NewGuid().ToString();
            var device = CreateDeviceFromRequest(deviceRequest, deviceType, id);
            var createdDevice = _storage.Add(device);
            return DeviceToDto(createdDevice);
        }

        public virtual IEnumerable<DeviceDTO> GetDevices(string deviceType)
        {
            return _storage.GetDevices(deviceType).Select(device => DeviceToDto(device));
        }

        public DeviceDTO GetDevice(string id)
        {
            var device = _storage.GetDevice(id);
            if (device == null)
                return null;
            return DeviceToDto(device);
        }

        public bool Delete(string id)
        {
            return _storage.Delete(id);
        }

        public DeviceDTO Update(DeviceRequest deviceRequest)
        {
            if (!Enum.IsDefined(typeof(EDeviceType), deviceRequest.DeviceType)) throw new UnsupportedDeviceTypeException($"The device type {deviceRequest.DeviceType} does not exists");
            if (IsDuplicated(deviceRequest.SerialNumber, deviceRequest.Id)) throw new DuplicatedDeviceException($"The device you try to add is already created");
            var deviceType = (EDeviceType)Enum.Parse(typeof(EDeviceType), deviceRequest.DeviceType, true);
            var device = CreateDeviceFromRequest(deviceRequest, deviceType, deviceRequest.Id);
            var updated = _storage.Update(device);
            if (updated)
                return DeviceToDto(device);
            return null;
        }

        /// <summary>
        /// Check if device is already created
        /// </summary>
        /// <param name="serialNumber">Serial number of the device</param>
        /// <param name="id">If is an update, id of the device to update</param>
        /// <returns>true if duplicated, otherwise false</returns>
        private bool IsDuplicated(string serialNumber, string id=null)
        {
            var devices = _storage.GetDevices(null);
            if (id != null)
            {
                var deviceDuplicated = devices.FirstOrDefault(device => device.SerialNumber == serialNumber);
                if (deviceDuplicated == null || deviceDuplicated?.Id == id)
                    return false;
                return true;
            }
            var serialNumbers = devices.Select(device => device.SerialNumber);
            if (serialNumbers.Contains(serialNumber))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Convert a Device to DeviceDTO
        /// </summary>
        /// <param name="device">Device to be converted to DTO</param>
        /// <returns>DeviceDTO converted</returns>
        private static DeviceDTO DeviceToDto(Device device)
        {
            return new DeviceDTO
            {
                Id = device.Id,
                SerialNumber = device.SerialNumber,
                FirmwareVersion = device.FirmwareVersion,
                DeviceType = device.DeviceType.ToString(),
                State = device.State,
                Ip = device.Ip,
                Port = device.Port
            };
        }

        /// <summary>
        /// Create a device object from DeviceRequest
        /// </summary>
        /// <param name="deviceRequest">Request received</param>
        /// <param name="deviceType">Device type</param>
        /// <param name="id">Id of the device object must be created</param>
        /// <returns>Device object created</returns>
        private static Device CreateDeviceFromRequest(DeviceRequest deviceRequest, EDeviceType deviceType, string id)
        {
            return new Device
            {
                Id = id,
                SerialNumber = deviceRequest.SerialNumber,
                FirmwareVersion = deviceRequest.FirmwareVersion,
                DeviceType = deviceType,
                State = deviceRequest.State,
                Ip = deviceType == EDeviceType.Gateway ? deviceRequest.Ip : null,
                Port = deviceType == EDeviceType.Gateway ? deviceRequest.Port : null
            };
        }
    }
}

