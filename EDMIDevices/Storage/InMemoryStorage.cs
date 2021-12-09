using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using EDMIDevices.Models.DAO;

namespace EDMIDevices.Storage
{
    public class InMemoryStorage<T> : IStorage<T> where T : BaseDevice
    {
        protected ConcurrentDictionary<string, T> _devices = new ConcurrentDictionary<string, T>();

        public InMemoryStorage()
        {
        }

        public T Add(T device)
        {
            _devices[device.Id] = device;
            return _devices[device.Id];
        }

        public bool Delete(string id)
        {
            return _devices.TryRemove(id, out var output);
        }

        public virtual T GetDevice(string id)
        {
            if (!_devices.ContainsKey(id))
                return null;
            return _devices[id];
        }

        public virtual IEnumerable<T> GetDevices(string deviceType)
        {
            if (deviceType != null)
            {
                return _devices.Select(e => e.Value)
                    .Where(e => e.DeviceType.ToString() == deviceType);
            }
            return _devices.Select(e => e.Value);
        }

        public bool Update(T device)
        {
            if (!_devices.ContainsKey(device.Id))
                return false;
            _devices[device.Id] = device;
            return true;
        }
    }
}

