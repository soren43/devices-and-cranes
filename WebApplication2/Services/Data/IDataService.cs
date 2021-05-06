using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Model;

namespace WebApplication2.Services.Data
{
    public interface IDataService
    {
        public List<Device> GetAllDevices();
        public List<Device> GetDevices(bool deleteStatus);
        public Device GetDeviceById(string id);
        public Device GetDeviceBySN(string s_n);
        public Device GetDeviceByManyIds(string id, string s_n, string model);
        public bool UpdateDevice(Device device);
        public bool RestoreDevice(Device device);
        public bool AddNewDevice(Device newDevice);
        public bool CraneIdValidation(string crane_id);
        public bool ValidateProperties(Device device);
        public bool DeleteDevice(string id);
        
    }
}
