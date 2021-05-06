using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication2.Services.Configuration;
using WebApplication2.Services.Data;

namespace UnitTest
{
    [TestClass]
    public class DataServiceTests
    {
        private Mock<IConfigurationService> _mockConfigurationService = new Mock<IConfigurationService>();

        [TestMethod]
        public void Should_GetAllDevices_ThrowException_If_Wrong_DevicesJsonPath()
        {
            // Arrange
            _mockConfigurationService.Setup(config => config.GetDevicesJsonPath()).Returns("badPath");
            DataService dataService = new DataService(_mockConfigurationService.Object);

            // Assert   
            Assert.ThrowsException<FileNotFoundException>(() => dataService.GetAllDevices());

        }

        [TestMethod]
        public void Should_CraneIdValidation_ThrowException_If_Wrong_CranesJsonPath()
        {
            // Arrange
            _mockConfigurationService.Setup(config => config.GetCranesJsonPath()).Returns("badPath");
            DataService dataService = new DataService(_mockConfigurationService.Object);

            // Assert   
            Assert.ThrowsException<FileNotFoundException>(() => dataService.CraneIdValidation("craneId"));

        }

    }
}
