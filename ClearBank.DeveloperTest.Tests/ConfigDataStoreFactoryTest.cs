using ClearBank.DeveloperTest.Configuration;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Factory;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests
{
    [TestFixture]
    public class ConfigDataStoreFactoryTest
    {
        [Test]
        public void TestCreate_IncorrectDataStoreType()
        {
            var settings = new DataStoreSettings { DataStoreType = "foobar" };

            var factory = new ConfigDataStoreFactory(settings);
            var result = factory.Create();

            Assert.That(result, Is.InstanceOf<AccountDataStore>());
        }

        [Test]
        public void TestCreate_BackupDataStoreType()
        {
            var settings = new DataStoreSettings { DataStoreType = "Backup" };

            var factory = new ConfigDataStoreFactory(settings);
            var result = factory.Create();

            Assert.That(result, Is.InstanceOf<BackupAccountDataStore>());
        }
    }
}
