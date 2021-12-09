using System;
using System.Collections.Generic;
using System.Linq;
using EDMIDevices.Models.DAO;
using EDMIDevices.Models.Requests;

namespace EDMIDevicesConsole.App
{
    public class Menu
    {

        private readonly Handler _handler;

        public Menu()
        {
            _handler = new Handler();
        }
        public void MainMenu()
        {
            var continueLoop = true;
            while (continueLoop)
            {
                Console.Clear();
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1) Add Device");
                Console.WriteLine("2) Remove Device");
                Console.WriteLine("3) Update Device");
                Console.WriteLine("4) Exit");
                Console.Write("\r\nSelect an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddDevice();
                        break;
                    case "2":
                        RemoveDevice();
                        break;
                    case "3":
                        UpdateDevice();
                        break;
                    case "4":
                        continueLoop = false;
                        break;
                }
            }
        }

        public async void AddDevice()
        {
            Console.Clear();
            var device = CreateDeviceRequest();
            await _handler.AddDeviceHandler(device);
        }

        public async void RemoveDevice()
        {
            Console.Clear();
            Console.WriteLine("Enter the ID of the device to delete");
            var id = Console.ReadLine();
            await _handler.RemoveDeviceHandler(id);
        }

        public async void UpdateDevice()
        {
            Console.Clear();
            Console.WriteLine("Enter the ID of the device to update");
            var id = Console.ReadLine();      
            var device = CreateDeviceRequest();
            device.Id = id;
            await _handler.UpdateDeviceHandler(device);
        }

        public string ShowDeviceTypes()
        {
            List<int> types = new List<int>
            {
                1, 2, 3
            };
            Console.WriteLine("Please select the device type:");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Water Meter");
            Console.WriteLine("2) Electric Meter");
            Console.WriteLine("3) Gateway");
            Console.Write("\r\nSelect an option: ");

            while (true)
            {
                var option = Convert.ToInt32(Console.ReadLine());
                if (!types.Contains(option))
                {
                    Console.WriteLine("Please, select a correct option");
                    continue;
                }
                return ((EDeviceType)option-1).ToString();
            }
        }

        private DeviceRequest CreateDeviceRequest()
        {
            var deviceType = ShowDeviceTypes();
            Console.Clear();
            Console.WriteLine("Enter the serialNumber");
            var serialNumber = Console.ReadLine();
            Console.WriteLine("Enter the firmwareVersion");
            var firmwareVersion = Console.ReadLine();
            Console.WriteLine("Enter the State");
            var state = Console.ReadLine();
            Console.WriteLine("Enter the IP");
            var ip = Console.ReadLine();
            Console.WriteLine("Enter the Port");
            var port = Convert.ToInt32(Console.ReadLine());
            DeviceRequest device = new DeviceRequest
            {
                DeviceType = deviceType,
                SerialNumber = serialNumber,
                FirmwareVersion = firmwareVersion,
                State = state,
                Ip = ip,
                Port = port
            };
            return device;
        }
    }
}


