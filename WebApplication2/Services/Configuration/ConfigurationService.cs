using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebApplication2.Model;

namespace WebApplication2.Services.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private Dictionary<string, string> configData = new Dictionary<string, string>();

        private const string HTTP_PORT = "HTTP_PORT";
        private const string CRANES_JSON = "CRANES_JSON";
        private const string DEVICES_JSON = "DEVICES_JSON";

        private readonly string[] environmentVariables = new string[] { HTTP_PORT, CRANES_JSON, DEVICES_JSON };

        public bool Initialize()
        {

            foreach (string envName in environmentVariables)
            {
                string value = Environment.GetEnvironmentVariable(envName);

                // Check whether the environment variable exists.
                if (value == null)
                {
                    return false;

                }

                this.configData.Add(envName, value);
            }

            return true;
        }

        public string GetDevicesJsonPath()
        {
            return this.configData[DEVICES_JSON];
        }

        public string GetCranesJsonPath()
        {
            return configData[CRANES_JSON];
        }

        public string GetHttpPort()
        {
            return configData[HTTP_PORT];
        }

              
    }
}
