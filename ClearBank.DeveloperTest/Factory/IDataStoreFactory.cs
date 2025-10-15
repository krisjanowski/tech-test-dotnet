using ClearBank.DeveloperTest.Data;
namespace ClearBank.DeveloperTest.Factory
{
    public interface IDataStoreFactory
    {
        IAccountDataStore Create();
    }
}
