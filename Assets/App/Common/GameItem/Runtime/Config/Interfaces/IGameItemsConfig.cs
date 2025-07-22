using System.Collections.Generic;

namespace App.Common.GameItem.Runtime.Config.Interfaces
{
    public interface IGameItemsConfig
    {
        IReadOnlyList<IGameItemConfig> Configs { get; }
    }
}