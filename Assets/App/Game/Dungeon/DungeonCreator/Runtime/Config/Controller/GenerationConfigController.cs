using System.Linq;
using App.Common.Configs.Runtime;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Config.Config;
using App.Game.Dungeon.DungeonCreator.Runtime.Config.Converter;
using App.Game.Dungeon.DungeonCreator.Runtime.Config.Loader;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Generation;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Config.Controller
{
    public class GenerationConfigController
    {
        private readonly IConfigLoader m_ConfigLoader;
        private GenerationsConfig m_Config;

        public GenerationConfigController(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public bool Initialize()
        {
            var loader = new GenerationConfigLoader(m_ConfigLoader);
            var converter = new GenerationDtoToConfigConverter();
            var dto = loader.Load();
            if (!dto.HasValue)
            {
                return false;
            }

            var config = converter.Convert(dto.Value);
            if (!config.HasValue)
            {
                return false;
            }

            m_Config = config.Value;

            return true;
        }

        public Optional<DungeonGenerationConfig> GetGeneration(string generationKey = "Default")
        {
            var config = m_Config.Configs.FirstOrDefault(x => x.Key == generationKey);
            if (config == default)
            {
                HLogger.LogError("Config not found.");
                return Optional<DungeonGenerationConfig>.Fail();
            }
            
            return Optional<DungeonGenerationConfig>.Success(config);
        }
    }
}