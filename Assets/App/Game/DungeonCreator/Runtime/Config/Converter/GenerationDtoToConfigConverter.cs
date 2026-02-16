using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;
using App.Game.GameManagers.External.Config.Dto;
using App.Game.GameManagers.External.Config.Model;
using App.Generation.DungeonGenerator.External;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Generation;

namespace App.Game.GameManagers.External.Config.Converter
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