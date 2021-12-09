using System;
using System.Net.Http;
using EDMIDevices.Exceptions;
using EDMIDevices.Models.DAO;
using EDMIDevices.Models.DTO;
using EDMIDevices.Models.Requests;
using EDMIDevices.Repositories;
using EDMIDevices.Storage;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace EDMIDevicesAPITest
{
    public class InMemoryStorageTest
    {
        private readonly IDeviceRepository _deviceRepository;

        public InMemoryStorageTest()
        {
            var inMemoryStorage = new InMemoryStorage<Device>();
            _deviceRepository = new DeviceRepository(inMemoryStorage);
        }

        [Fact]
        public void Task_GetPostById_Return_NotFoundResult()
        {
            var id = DummyData.GenerateRandomId();

            Assert.Null(_deviceRepository.GetDevice(id));
        }

        [Fact]
        public void Task_Add_Device_Return_OkResult()
        {
            var device = DummyData.CreateDeviceRequestDummy("serial");

            var deviceAdded = _deviceRepository.Add(device);

            Assert.IsType<DeviceDTO>(deviceAdded);

            Assert.IsType<DeviceDTO>(_deviceRepository.GetDevice(deviceAdded.Id));
        }

        [Fact]
        public void Task_Add_InvalidData_Return_BadRequest()
        {
            var device = DummyData.CreateDeviceRequestDummyNotValid();

            Assert.Throws<NullReferenceException>(() => _deviceRepository.Add(device));
        }

        [Fact]
        public void Task_Add_InvalidData_Return_ThrowUnsupportedDeviceTypeException()
        {
            var device = DummyData.CreateDeviceRequestDummyNotValidDeviceType();

            Assert.Throws<UnsupportedDeviceTypeException>(() => _deviceRepository.Add(device));
        }

        [Fact]
        public void Task_Add_InvalidData_Return_ThrowDuplicatedDeviceException()
        {
            var device = DummyData.CreateDeviceRequestDummy("serialTwo");

            _deviceRepository.Add(device);

            Assert.Throws<DuplicatedDeviceException>(() => _deviceRepository.Add(device));
        }


        [Fact]
        public void Task_Update_ValidData_Return_OkResult()
        {
            var deviceRequest = DummyData.CreateDeviceRequestDummy("serialThree");

            var deviceCreated = _deviceRepository.Add(deviceRequest);

            deviceRequest.Id = deviceCreated.Id;
            deviceRequest.FirmwareVersion = "New Firmware Updated";

            Assert.IsType<DeviceDTO>(_deviceRepository.Update(deviceRequest));
        }

        [Fact]
        public void Task_Update_InvalidData_Return_NotFound()
        {
            var deviceRequest = DummyData.CreateDeviceRequestDummy("serialFour");
            var id = DummyData.GenerateRandomId();
            deviceRequest.Id = id;

            Assert.Null(_deviceRepository.Update(deviceRequest));
        }

        [Fact]
        public void Task_Delete_Post_Return_OkResult()
        {
            var deviceRequest = DummyData.CreateDeviceRequestDummy("serialFive");

            var deviceCreated = _deviceRepository.Add(deviceRequest);

            Assert.True(_deviceRepository.Delete(deviceCreated.Id));
        }

        [Fact]
        public void Task_Delete_Post_Return_NotFoundResult()
        {
            var id = DummyData.GenerateRandomId();

            Assert.False(_deviceRepository.Delete(id));
        }
    }
}

