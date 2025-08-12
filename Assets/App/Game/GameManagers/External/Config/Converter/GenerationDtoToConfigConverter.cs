using App.Common.Utilities.Utility.Runtime;
using App.Game.GameManagers.External.Config.Dto;
using App.Game.GameManagers.External.Config.Model;
using App.Generation.DungeonGenerator.External;

namespace App.Game.GameManagers.External.Config.Converter
{
    public class GenerationDtoToConfigConverter
    {
        private readonly DungeonGenerationDtoToConfigConverter m_DungeonGenerationDtoToConfigConverter;

        public GenerationDtoToConfigConverter()
        {
            m_DungeonGenerationDtoToConfigConverter = new DungeonGenerationDtoToConfigConverter();
        }

        public Optional<GenerationConfig> Convert(GenerationConfigDto dto)
        {
            var generationConfig = m_DungeonGenerationDtoToConfigConverter.Convert(dto.Generation);
            
            var config = new GenerationConfig(generation: generationConfig);
            return Optional<GenerationConfig>.Success(config);
        }
    }
}