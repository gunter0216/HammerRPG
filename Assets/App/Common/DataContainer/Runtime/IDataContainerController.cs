using App.Common.Utility.Runtime;

namespace App.Common.DataContainer.Runtime
{
    public interface IDataContainerController
    {
        Optional<DataReference> AddData(string key, object data);
        Optional<DataReference> RemoveData(string key, object data);
        Optional<object> GetData(IDataReference dataReference);
    }
}