using System.Configuration;
using NUnit.Framework;

namespace DalSoft.Configuration.Test.Unit
{
    [TestFixture]
    public class JsonConfigTests
    {
        [Test]
        public void GetSetting_ValidJsonConfig_ReturnStronglyTypedConfig()
        {
            var config = ValidConfig.GetSettings();
            Assert.That(config.Website, Is.EqualTo("http://dalsoft.co.uk/"));
            Assert.That(config.DatabaseConnectionString, Is.EqualTo("Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;"));
        }

        [Test]
        public void GetSetting_ValidJsonConfigWithComposition_ReturnsCompositeValues()
        {
            var config = ValidConfig.GetSettings();
            Assert.That(config.MyAppConfig.Website, Is.EqualTo("http://dalsoft.co.uk/"));
            Assert.That(config.MyAppConfig.DatabaseConnectionString, Is.EqualTo("Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;"));
        }

        [Test]
        public void GetSetting_ValidJsonConfigWithList_ReturnsListValues()
        {
            var config = ValidConfig.GetSettings();
            Assert.That(config.ValidList[0].Website, Is.EqualTo("http://dalsoft.co.uk/"));
            Assert.That(config.ValidList[1].Website, Is.EqualTo("http://google.co.uk/"));
        }

        [Test]
        public void GetSetting_MissingConfig_ThrowsConfigurationErrorsException()
        {
            var ex = Assert.Throws<ConfigurationErrorsException>(()=>MissingConfig.GetSettings());
            Assert.That(ex.Message, Is.StringContaining("setting is missing from your config"));
        }

        [Test]
        public void GetSetting_InvalidJson_ThrowsConfigurationErrorsException()
        {
            var ex = Assert.Throws<ConfigurationErrorsException>(() => InvalidConfig.GetSettings());
            Assert.That(ex.Message, Is.StringContaining("invalid"));
        }
    }
}
