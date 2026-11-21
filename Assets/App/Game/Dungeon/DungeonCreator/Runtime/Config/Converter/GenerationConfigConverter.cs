using App.Common.Utilities.Utility.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Config.Config;
using System.Collections.Generic;
using App.Generation.DungeonGenerator.Runtime.Configs;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Generation;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Config.Converter
{
    public class GenerationConfigConverter
    {
        private readonly DungeonGenerationConfigConverter m_DungeonGenerationConfigConverter;

        public GenerationConfigConverter()
        {
            m_DungeonGenerationConfigConverter = new DungeonGenerationConfigConverter();
        }

        public Optional<GenerationsConfig> Convert(GenerationConfig asset)
        {
            if (asset == null || asset.Generations == null || asset.Generations.Length == 0)
                return Optional<GenerationsConfig>.Fail();

            var keys = new HashSet<string>();
            var generations = new DungeonGenerationConfig[asset.Generations.Length];
            for (int i = 0; i < asset.Generations.Length; ++i)
            {
                var settings = asset.Generations[i];
                if (settings == null || string.IsNullOrWhiteSpace(settings.Key) || !keys.Add(settings.Key) ||
                    !m_DungeonGenerationConfigConverter.TryConvert(settings, out var config))
                    return Optional<GenerationsConfig>.Fail();

                generations[i] = config;
            }

            var generation = new GenerationsConfig(generations);
            
            return Optional<GenerationsConfig>.Success(generation);
        }

    }
}
