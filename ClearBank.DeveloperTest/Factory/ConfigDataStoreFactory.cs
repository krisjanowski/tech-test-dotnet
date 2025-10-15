using ClearBank.DeveloperTest.Configuration;
using ClearBank.DeveloperTest.Data;
namespace ClearBank.DeveloperTest.Factory
{
    public class ConfigDataStoreFactory
    {
        private readonly DataStoreSettings DataStoreSettings;

        public ConfigDataStoreFactory(DataStoreSettings dataStoreSettings)
        {
            DataStoreSettings = dataStoreSettings;
        }

        public IAccountDataStore Create()
        {
            if (DataStoreSettings.DataStoreType == "Backup")
            {
                return new BackupAccountDataStore();
            }
            else
            {
                return new AccountDataStore();
            }
        }
    }
}
