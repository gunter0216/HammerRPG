using App.Common.Utility.Runtime;

namespace App.Common.DataContainer.Runtime.Data
{
    public interface IContainerDataController
    {
        Optional<DataReference> AddData(string key, object data);
        Optional<DataReference> RemoveData(string key, object data);
        Optional<object> GetData(IDataReference dataReference);
        Optional<T> GetData<T>(IDataReference dataReference) where T : class;
    }
}