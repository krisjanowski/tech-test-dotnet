using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Factory;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Collections.Generic;

namespace ClearBank.DeveloperTest.Tests
{
    [TestFixture]
    public class ConfigDataStoreFactoryTest
    {
        [Test]
        public void TestCreate_IncorrectDataStoreType()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "DataStoreSettings:DataStoreType", "foobar" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var factory = new ConfigDataStoreFactory(configuration);
            var result = factory.Create();

            Assert.That(result, Is.InstanceOf<AccountDataStore>());
        }

        [Test]
        public void TestCreate_BackupDataStoreType()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "DataStoreSettings:DataStoreType", "Backup" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var factory = new ConfigDataStoreFactory(configuration);
            var result = factory.Create();

            Assert.That(result, Is.InstanceOf<BackupAccountDataStore>());
        }
    }
}
