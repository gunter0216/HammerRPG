using System.Collections.Generic;

namespace App.Common.ModuleItem.Runtime.Config.Interfaces
{
    public interface IModuleItemsConfig
    {
        IReadOnlyList<IModuleItemConfig> Configs { get; }
    }
}