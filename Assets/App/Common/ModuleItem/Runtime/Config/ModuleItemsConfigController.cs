using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime.Config
{
    public class ModuleItemsConfigController : IGameItemConfigController
    {
        private readonly IReadOnlyList<IModuleItemConfig> m_ListConfigs;

        private Dictionary<string, IModuleItemConfig> m_Configs;

        public ModuleItemsConfigController(IGameItemsConfig config)
        {
            m_ListConfigs = config.Configs;
        }

        public bool Initialize()
        {
            m_Configs = new Dictionary<string, IModuleItemConfig>(m_ListConfigs.Count);
            for (int i = 0; i < m_ListConfigs.Count; ++i)
            {
                var config = m_ListConfigs[i];
                m_Configs.Add(config.Id, config);
            }

            return true;
        }

        public Optional<IModuleItemConfig> GetConfig(string id)
        {
            if (m_Configs.TryGetValue(id, out var config))
            {
                return Optional<IModuleItemConfig>.Success(config);
            }
            
            return Optional<IModuleItemConfig>.Fail();
        }
    }
}
