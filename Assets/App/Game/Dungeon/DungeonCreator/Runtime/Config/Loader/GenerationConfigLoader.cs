using App.Common.Configs.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.Configs;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Config.Loader
{
    public class GenerationConfigLoader
    {
        private readonly IConfigLoader m_ConfigLoader;

        public GenerationConfigLoader(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public Optional<GenerationConfig> Load()
        {
            return m_ConfigLoader.LoadGameConfig<GenerationConfig>(GenerationConfig.Address);
        }
    }
}
