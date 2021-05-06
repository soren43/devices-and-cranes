using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApplication2.Model;
using WebApplication2.Services.Configuration;

namespace WebApplication2.Services.Data
{
    public class DataService : IDataService
    {
        private readonly string devicesJsonPath;
        private readonly string cranesJsonPath;

        private const string DateFormat = "dd/MM/yyyy HH:mm:ss";

        public DataService(IConfigurationService configuration)
        {
            devicesJsonPath = configuration.GetDevicesJsonPath();
            cranesJsonPath = configuration.GetCranesJsonPath();
        }

        private List<ServerDevice> GetAllServerDevices()
        {
            using (StreamReader r = new StreamReader(devicesJsonPath))
            {
                string json = r.ReadToEnd();

                var allDevices = JsonConvert.DeserializeObject<List<ServerDevice>>(json);
                return allDevices.ToList();

            }
        }

        public List<Device> GetAllDevices()
        {
            using (StreamReader r = new StreamReader(devicesJsonPath))
            {
                string json = r.ReadToEnd();

                var allDevices = JsonConvert.DeserializeObject<List<Device>>(json);
                return allDevices.ToList();

            }
        }

        public List<Device> GetDevices(bool deleteStatus)
        {
            var allDevices = GetAllServerDevices();
            var result =  allDevices.Where(device => device.deleted == deleteStatus).ToList();

            return Device.ConvertToDevices(result);
        }

        public Device GetDeviceById(string id)
        {
            var device = GetDevices(false);
            return device.Where(device => device.id == id).FirstOrDefault();
        }

        public Device GetDeviceBySN(string s_n)
        {
            var device = GetDevices(false);
            return device.Where(device => device.s_n == s_n).FirstOrDefault();
        }

        public Device GetDeviceByManyIds(string id, string s_n, string model)
        {
            var device = GetDevices(false);
            return device.Where(device => device.id == id && device.s_n == s_n && device.model == model).FirstOrDefault();
        }

        public bool AddNewDevice(Device newDevice)
        {

            string json = File.ReadAllText(devicesJsonPath);
            var list = JsonConvert.DeserializeObject<List<ServerDevice>>(json);
            newDevice.created = DateTime.UtcNow.ToString(DateFormat);
            newDevice.updated = newDevice.created;
            if(CraneIdValidation(newDevice.crane_id))
            {
                list.Add(Device.ConvertToServerDevice(newDevice, false));
                var newJson = JsonConvert.SerializeObject(list, Formatting.Indented);
                File.WriteAllText(devicesJsonPath, newJson);
                return true;
            }
            return false;
        }

        public bool UpdateDevice(Device updatedDevice)
        {
            string json = File.ReadAllText(devicesJsonPath); 
            var list = JsonConvert.DeserializeObject<List<ServerDevice>>(json);

            ServerDevice deviceToUpdate = list.Where(device => device.id == updatedDevice.id && !device.deleted).FirstOrDefault();
            var index = list.IndexOf(deviceToUpdate);

            if (index != -1 && CraneIdValidation(updatedDevice.crane_id))
            {
                updatedDevice.updated = DateTime.UtcNow.ToString(DateFormat);
                var updatedServerDevice = Device.ConvertToServerDevice(updatedDevice, false);
                list[index] = updatedServerDevice;
                var updatedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
                File.WriteAllText(devicesJsonPath, updatedJson);
                return true;
            }
            return false;
        }

        public bool RestoreDevice(Device deviceToRestore)
        {
            string json = File.ReadAllText(devicesJsonPath);
            var list = JsonConvert.DeserializeObject<List<ServerDevice>>(json);

            ServerDevice serverDeviceToRestore = list.Where(device => device.id == device.id).FirstOrDefault();
            var index = list.IndexOf(serverDeviceToRestore);

            if (index != -1)
            {
                deviceToRestore.updated = DateTime.UtcNow.ToString(DateFormat);
                var restoreServerDevice = Device.ConvertToServerDevice(deviceToRestore, false);
                list[index] = restoreServerDevice;
                var updatedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
                File.WriteAllText(devicesJsonPath, updatedJson);
                return true;
            }
            return false;
        }
            
        public bool CraneIdValidation(string crane_id)
        {
            using (StreamReader r = new StreamReader(cranesJsonPath))
            {
                string json = r.ReadToEnd();

                var allCraneIds = JsonConvert.DeserializeObject<string[]>(json);
                return allCraneIds.Contains(crane_id);
            }
        }

        public bool ValidateProperties(Device device)
        {
           return typeof(Device).GetProperties().All(propertyInfo => propertyInfo.GetValue(device) != null);
        }

        public bool DeleteDevice(string id)
        {
            string json = File.ReadAllText(devicesJsonPath);
            var list = JsonConvert.DeserializeObject<List<ServerDevice>>(json);

            ServerDevice deviceToDelete = list.Where(device => device.id == id).FirstOrDefault();
            var index = list.IndexOf(deviceToDelete);

            if (index != -1)
            {
                list[index].updated = DateTime.UtcNow.ToString(DateFormat);
                list[index].deleted = true;
                var updatedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
                File.WriteAllText(devicesJsonPath, updatedJson);
                return true;
            }
            return false;
        }
    }
}
