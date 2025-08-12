using App.Common.Utilities.Utility.Runtime;

namespace App.Common.Data.Runtime
{
    public interface IDataManager
    {
        void SaveProgress();
        Optional<IData> GetData(string name);
        Optional<T> GetData<T>(string name) where T : IData;
    }
}