using System;
using System.Collections.Generic;
using EDMIDevices.Models.DTO;
using EDMIDevices.Models.Requests;

namespace EDMIDevices.Repositories
{
    public interface IDeviceRepository
    {
        /// <summary>
        /// Create a new device
        /// </summary>
        /// <param name="deviceRequest">Request received</param>
        /// <returns>Device created, if fail, null</returns>
        DeviceDTO Add(DeviceRequest deviceRequest);

        /// <summary>
        /// Find all devices
        /// </summary>
        /// <returns>IEnumerable of devices, if no devices found null</returns>
        IEnumerable<DeviceDTO> GetDevices(string deviceType);

        /// <summary>
        /// Find a device by id
        /// </summary>
        /// <param name="id">Id of the device</param>
        /// <returns>the device if found, otherwise null</returns>
        DeviceDTO GetDevice(string id);

        /// <summary>
        /// Delete a device by id
        /// </summary>
        /// <param name="id">Id of the device to delete</param>
        /// <returns>true if deleted, otherwise false</returns>
        bool Delete(string id);

        /// <summary>
        /// Update a device by id
        /// </summary>
        /// <param name="deviceRequest">Request received</param>
        /// <returns>Device updated if ok, otherwise null</returns>
        DeviceDTO Update(DeviceRequest deviceRequest);
    }
}

