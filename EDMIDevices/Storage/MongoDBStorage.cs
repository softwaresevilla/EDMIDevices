using System;
using System.Collections.Generic;
using EDMIDevices.Models.Config;
using EDMIDevices.Models.DAO;
using MongoDB.Driver;

namespace EDMIDevices.Storage
{
    public class MongoDBStorage<T> : IStorage<T> where T : BaseDevice
    {
        private readonly IMongoCollection<T> _devices;

        public MongoDBStorage(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _devices = database.GetCollection<T>(settings.CollectionName);
        }

        public T Add(T device)
        {
            _devices.InsertOne(device);
            return device;
        }

        public bool Delete(string id)
        {
            var deleteResult = _devices.DeleteOne(device => device.Id == id);
            return deleteResult.DeletedCount == 1 ? true : false;
        }

        public virtual T GetDevice(string id)
        {
            return _devices.Find<T>(device => device.Id == id).FirstOrDefault();
        }

        public virtual IEnumerable<T> GetDevices(string deviceType)
        {
            if (deviceType != null)
            {
                return _devices.Find(device => device.DeviceType.ToString() == deviceType).ToList();
            }
            return _devices.Find(device => true).ToList();
        }

        public bool Update(T device)
        {
            var updateResult = _devices.ReplaceOne(deviceOnDb => deviceOnDb.Id == device.Id, device);
            return updateResult.ModifiedCount == 1 ? true : false;
        }
    }
}