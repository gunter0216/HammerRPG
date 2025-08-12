using System.Collections.Generic;

namespace Assets.App.Common.ModuleItem.Runtime.Config.Interfaces
{
    public interface IModuleItemsConfig
    {
        IReadOnlyList<IModuleItemConfig> Configs { get; }
    }
}