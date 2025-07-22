using System.Collections.Generic;
using App.Common.GameItem.Runtime.Config.Interfaces;
using App.Common.Utility.Runtime;

namespace App.Common.GameItem.Runtime.Config
{
    public class GameItemConfigController : IGameItemConfigController
    {
        private readonly IReadOnlyList<IGameItemConfig> m_ListConfigs;

        private Dictionary<string, IGameItemConfig> m_Configs;

        public GameItemConfigController(IGameItemsConfig config)
        {
            m_ListConfigs = config.Configs;
        }

        public bool Initialize()
        {
            m_Configs = new Dictionary<string, IGameItemConfig>(m_ListConfigs.Count);
            for (int i = 0; i < m_ListConfigs.Count; ++i)
            {
                var config = m_ListConfigs[i];
                m_Configs.Add(config.Id, config);
            }

            return true;
        }

        public Optional<IGameItemConfig> GetConfig(string id)
        {
            if (m_Configs.TryGetValue(id, out var config))
            {
                return Optional<IGameItemConfig>.Success(config);
            }
            
            return Optional<IGameItemConfig>.Fail();
        }
    }
}
