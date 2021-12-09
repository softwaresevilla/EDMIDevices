using System;

namespace EDMIDevices.Models.Config
{
    public enum EStorageDestination
    {
        MongoDB,
        Memory
    }

    public class StorageSettings
    {
        public EStorageDestination Destination { get; set; }
    }
}
