using System.Collections.Generic;

namespace App.Common.ModuleItem.Runtime.Config.Interfaces
{
    public interface IGameItemsConfig
    {
        IReadOnlyList<IModuleItemConfig> Configs { get; }
    }
}