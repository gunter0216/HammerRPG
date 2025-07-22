using System.Collections;
using App.Common.Data.Runtime;

namespace App.Common.DataContainer.Runtime.Data
{
    public interface IContainerData : IData
    {
        IList Data { get; }
        string GetContainerKey();
    }
}