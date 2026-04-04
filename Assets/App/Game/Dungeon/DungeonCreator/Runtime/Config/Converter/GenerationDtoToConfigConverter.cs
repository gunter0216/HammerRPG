using App.Common.Utilities.Utility.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Config.Config;
using App.Game.Dungeon.DungeonCreator.Runtime.Config.Dto;
using App.Generation.DungeonGenerator.External;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Generation;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Config.Converter
{
    public class GenerationDtoToConfigConverter
    {
        private readonly DungeonGenerationDtoToConfigConverter m_DungeonGenerationDtoToConfigConverter;

        public GenerationDtoToConfigConverter()
        {
            m_DungeonGenerationDtoToConfigConverter = new DungeonGenerationDtoToConfigConverter();
        }

        public Optional<GenerationsConfig> Convert(GenerationConfigDto dto)
        {
            var generations = new DungeonGenerationConfig[dto.Generations.Length];
            for (int i = 0; i < dto.Generations.Length; ++i)
            {
                var config = m_DungeonGenerationDtoToConfigConverter.Convert(dto.Generations[i]);
                generations[i] = config;
            }

            var generation = new GenerationsConfig(generations);
            
            return Optional<GenerationsConfig>.Success(generation);
        }
    }
}