using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Model;
using WebApplication2.Services.Data;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly IDataService _dataService;

        public DevicesController( IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public IActionResult Get() 
        {
            var devices = _dataService.GetDevices(false);
            return Ok(devices);
        }

        [HttpGet]
        [Route("deleted")]
        public IActionResult GetDeletedDevices()
        {
            var devices = _dataService.GetDevices(true);
            return Ok(devices);
        }


        [HttpPost]
        public IActionResult Post(Device device)
        {
            // If a property is missing the operation should fail
            if (!_dataService.ValidateProperties(device))
            {
                return BadRequest();
            }

            Device exactlySameDevice = _dataService.GetDeviceByManyIds(device.id, device.s_n, device.model);
            if (exactlySameDevice != null)
            {
                bool updatedOk = _dataService.UpdateDevice(device);
                if (updatedOk)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("ERROR - missing device OR craneId is not valid");
                }
            }

            Device deviceWithSameId = _dataService.GetDeviceById(device.id);
            Device deviceWithSameSN = _dataService.GetDeviceBySN(device.s_n);
            if (deviceWithSameId != null || deviceWithSameSN != null)
            {
                return Conflict("device With Same Id or S_N already exists");
            }
            
             // Otherwise, create a new device
            bool addedOk = _dataService.AddNewDevice(device);
            if (addedOk)
            {
                return Ok();
            }
            else
            {
                return BadRequest("craneId is not valid");
            }        
        }

     
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(string id)
        {

            var device = _dataService.GetDeviceById(id);
            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put(string id, [FromBody] Device device)//  TODO [FromUri] queryParameters)
        {
            device.id = id;
            bool updatedOk = _dataService.UpdateDevice(device);
            if (updatedOk)
            {
                return Ok();
            }
            else
            {
                return NotFound("ERROR - can't update missing device");
            }

        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(string id)
        {
            bool deletedOk = _dataService.DeleteDevice(id);
            if (deletedOk)
            {
                return Ok();
            }
            else
            {
                return NotFound("ERROR - can't delete missing device");
            }
        }

        [HttpPost]
        [Route("deleted/{id}/restore")]
        public IActionResult RestoreDevice(string id)
        {
            // check if already exist
            IEnumerable<Device> isDeviceExist = _dataService.GetAllDevices().Where(device => device.id == id);
            if (isDeviceExist.Any())
            {
                // restore device
                Device deviceToRestore = isDeviceExist.First();
                
                bool restoredOk = _dataService.RestoreDevice(deviceToRestore);
                if (restoredOk)
                {
                    return Ok();
                }
                else
                {
                    return NotFound("ERROR - can't restore missing device");
                }
            }

            return NotFound("ERROR - can't restore missing device");
        }
    }
       
}

