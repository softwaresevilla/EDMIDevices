using System;
namespace EDMIDevices.Exceptions
{
    public class UnsupportedDeviceTypeException : Exception
    {
        public UnsupportedDeviceTypeException() { }

        public UnsupportedDeviceTypeException(string msg) : base(msg)
        {
        }
    }

    public class DuplicatedDeviceException : Exception
    {
        public DuplicatedDeviceException() { }

        public DuplicatedDeviceException(string msg) : base(msg)
        {
        }
    }
}
