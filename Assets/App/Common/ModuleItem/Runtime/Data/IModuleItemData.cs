using System.Collections.Generic;
using App.Common.DataContainer.Runtime;

namespace Assets.App.Common.ModuleItem.Runtime.Data
{
    public interface IModuleItemData
    {
        string Id { get; }
        List<DataReference> ModuleRefs { get; }
    }
}