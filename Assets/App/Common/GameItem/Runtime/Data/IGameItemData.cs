using System.Collections.Generic;
using App.Common.DataContainer.Runtime;

namespace App.Common.GameItem.Runtime.Data
{
    public interface IGameItemData
    {
        string Id { get; }
        List<DataReference> ModuleRefs { get; }
    }
}