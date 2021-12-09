using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using EDMIDevices.Models;
using EDMIDevices.Storage;
using EDMIDevices.Exceptions;
using EDMIDevices.Models.Requests;
using EDMIDevices.Models.DAO;
using EDMIDevices.Models.DTO;
using EDMIDevices.Repositories;

namespace EDMIDevices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceRepository _repository;

        public DeviceController(IDeviceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetDevices([FromQuery]string deviceType)
        {
            try
            {
                return Ok(_repository.GetDevices(deviceType));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetDevice(string id)
        {
            try
            {
                var device = _repository.GetDevice(id);
                if (device == null)
                {
                    return NotFound("Device not found");
                }
                return Ok(device);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Add(DeviceRequest deviceRequest)
        {
            try
            {
                if (ModelState.IsValid)
                    return Ok(_repository.Add(deviceRequest));
                return BadRequest(ModelState);
            }
            catch (UnsupportedDeviceTypeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicatedDeviceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] DeviceRequest deviceRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var updated = _repository.Update(deviceRequest);
                    if (updated != null)
                        return Ok(updated);
                    return NotFound("No device found");
                }
                return BadRequest(ModelState);
            }
            catch (UnsupportedDeviceTypeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicatedDeviceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                var deleted = _repository.Delete(id);
                if (deleted)
                {
                    return Ok(id);
                }
                return NotFound("No device found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}