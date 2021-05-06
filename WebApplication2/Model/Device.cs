using System.Collections.Generic;

namespace WebApplication2.Model
{
    public class Device
    {
        public string id { get; set; }
        public string crane_id { get; set; }
        public string s_n { get; set; }
        public string model { get; set; }
        public string description { get; set; }
        public string created { get; set; } // CONSIDER CHANGING TO DATE TYPE
        public string updated { get; set; } // CONSIDER CHANGING TO DATE TYPE

        //public bool deleted { get; set; } // this parameter is private. ServerDevice contains it 

        public static List<Device> ConvertToDevices(List<ServerDevice> serverDevices)
        {

            var devices =  new List<Device>();
            foreach (var serverDevice in serverDevices)
            {
                var device = (Device)serverDevice;
                //var device = new Device();
                //device.id = serverDevice.id;
                //device.crane_id = serverDevice.crane_id;
                //device.s_n = serverDevice.s_n;
                //device.model = serverDevice.model;
                //device.description = serverDevice.description;
                //device.created = serverDevice.created;
                //device.updated = serverDevice.updated;

                devices.Add(device);
            }           

            return devices;
        }

        public static ServerDevice ConvertToServerDevice(Device device, bool deleted)
        {
            var serverDevice = new ServerDevice();
            serverDevice.id = device.id;
            serverDevice.crane_id = device.crane_id;
            serverDevice.s_n = device.s_n;
            serverDevice.model = device.model;
            serverDevice.description = device.description;
            serverDevice.created = device.created;
            serverDevice.updated = device.updated;
            serverDevice.deleted = deleted;

            return serverDevice;
        }

    }
}
