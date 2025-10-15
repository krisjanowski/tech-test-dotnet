using ClearBank.DeveloperTest.Data;
using Microsoft.Extensions.Configuration;

namespace ClearBank.DeveloperTest.Factory
{
    public class ConfigDataStoreFactory
    {
        private readonly IConfiguration Configuration;

        public ConfigDataStoreFactory(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IAccountDataStore Create()
        {
            var type = Configuration["DataStoreSettings:DataStoreType"];

            if (type == "Backup")
            {
                return new BackupAccountDataStore();
            }

            return new AccountDataStore();
        }
    }
}
