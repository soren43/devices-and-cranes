using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Model;

namespace WebApplication2.Services.Configuration
{
    public interface IConfigurationService
    {
        public bool Initialize();

        public string GetDevicesJsonPath();
        public string GetCranesJsonPath();
        public string GetHttpPort();
    }
}
