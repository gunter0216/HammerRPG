using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.Runtime.Config
{
    public class ModuleItemsConfigController : IModuleItemConfigController
    {
        private Dictionary<string, IModuleItemConfig> m_Configs;
        private Dictionary<string, IReadOnlyList<IModuleItemConfig>> m_TypeToConfigs;

        public ModuleItemsConfigController()
        {
            m_TypeToConfigs = new Dictionary<string, IReadOnlyList<IModuleItemConfig>>();
        }

        public bool RegisterItems(IReadOnlyList<IModuleItemConfig> configs, string type)
        {
            m_Configs = new Dictionary<string, IModuleItemConfig>(configs.Count);
            for (int i = 0; i < configs.Count; ++i)
            {
                var config = configs[i];
                m_Configs.Add(config.Id, config);
            }
            
            m_TypeToConfigs.Add(type, configs);

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
        
        public Optional<IReadOnlyList<IModuleItemConfig>> GetConfigs(string type)
        {
            if (m_TypeToConfigs.TryGetValue(type, out var configs))
            {
                return Optional<IReadOnlyList<IModuleItemConfig>>.Success(configs);
            }
            
            return Optional<IReadOnlyList<IModuleItemConfig>>.Fail();
        }
    }
}
