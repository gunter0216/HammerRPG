using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Common.ModuleItem.Runtime.Config
{
    public class GameItemsConfig : IGameItemsConfig
    {
        private readonly IReadOnlyList<IModuleItemConfig> m_Configs;

        public IReadOnlyList<IModuleItemConfig> Configs => m_Configs;

        public GameItemsConfig(IReadOnlyList<IModuleItemConfig> configs)
        {
            m_Configs = configs;
        }
    }
}