using App.Common.Utility.Runtime;

namespace App.Common.Data.Runtime
{
    public interface IDataManager
    {
        void SaveByExit();
        Optional<IData> GetData(string name);
    }
}