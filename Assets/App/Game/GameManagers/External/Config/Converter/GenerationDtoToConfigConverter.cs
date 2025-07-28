using App.Common.Utility.Runtime;
using App.Game.GameManagers.External.Config.Dto;
using App.Game.GameManagers.External.Config.Model;

namespace App.Game.GameManagers.External.Config.Converter
{
    public class GenerationDtoToConfigConverter
    {
        public Optional<GenerationConfig> Convert(GenerationConfigDto dto)
        {
            var config = new GenerationConfig(generation: dto.Generation);
            return Optional<GenerationConfig>.Success(config);
        }
    }
}