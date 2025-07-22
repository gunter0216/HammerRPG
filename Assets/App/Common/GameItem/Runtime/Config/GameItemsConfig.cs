using System.Collections.Generic;
using App.Common.GameItem.Runtime.Config.Interfaces;

namespace App.Common.GameItem.Runtime.Config
{
    public class GameItemsConfig : IGameItemsConfig
    {
        private readonly IReadOnlyList<IGameItemConfig> m_Configs;

        public IReadOnlyList<IGameItemConfig> Configs => m_Configs;

        public GameItemsConfig(IReadOnlyList<IGameItemConfig> configs)
        {
            m_Configs = configs;
        }
    }
}