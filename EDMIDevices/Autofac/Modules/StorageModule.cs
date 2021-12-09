using Autofac;
using EDMIDevices.Models.Config;
using EDMIDevices.Models.DAO;
using EDMIDevices.Storage;
using Microsoft.Extensions.Configuration;

namespace EDMIDevices.Autofac.Modules
{
    public class StorageModule : Module
    {
        private readonly IConfiguration _configuration;

        public StorageModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            StorageSettings storageSettings = _configuration.GetSection("Storage").Get<StorageSettings>();
            if (storageSettings.Destination == EStorageDestination.Memory)
                builder.RegisterType<InMemoryStorage<Device>>().As<IStorage<Device>>()
                    .SingleInstance();
            if (storageSettings.Destination == EStorageDestination.MongoDB)
            {
                DatabaseSettings databaseSettings = _configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
                builder.RegisterType<MongoDBStorage<Device>>().As<IStorage<Device>>()
                    .WithParameter("settings", databaseSettings)
                    .SingleInstance();
            }                
        }
    }
}
