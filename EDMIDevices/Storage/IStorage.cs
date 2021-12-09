using System;
using System.Collections.Generic;
using EDMIDevices.Models.DAO;

namespace EDMIDevices.Storage
{
    public interface IStorage<T> where T : BaseDevice
    {
        /// <summary>
        /// Get all the devices
        /// </summary>
        /// <param name="deviceType">Optional device type</param>
        /// <returns>A list of devices</returns>
        IEnumerable<T> GetDevices(string deviceType);

        /// <summary>
        /// Get a device by the id
        /// </summary>
        /// <param name="id">Id of the device</param>
        /// <returns>The device if exists. If not exist, return null</returns>
        T GetDevice(string id);

        /// <summary>
        /// Add new device
        /// </summary>
        /// <param name="device">Device to add</param>
        /// <returns>The created device</returns>
        T Add(T device);

        /// <summary>
        /// Update an existing device
        /// </summary>
        /// <param name="device">Device to update</param>
        /// <returns>The updated entity</returns>
        bool Update(T device);

        /// <summary>
        /// Delete a device
        /// </summary>
        /// <param name="device">Device to delete</param>
        /// <returns>True if the device was deleted, otherwise false</returns>
        bool Delete(string id);
    }
}

