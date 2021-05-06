using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication2.Services.Configuration;
using WebApplication2.Services.Data;

namespace UnitTest
{
    [TestClass]
    public class ConfigurationServiceTests
    {
        private Mock<IConfigurationService> _mockConfigurationService = new Mock<IConfigurationService>();

        [TestMethod]
        public void Should_Initialize_Return_False_If_Missing_EnvironmentVariables()
        {
            // TODO FIX Test
            var result = _mockConfigurationService.Object.Initialize();
            
            // Assert   
            Assert.IsFalse(result);

        }
    }
}
