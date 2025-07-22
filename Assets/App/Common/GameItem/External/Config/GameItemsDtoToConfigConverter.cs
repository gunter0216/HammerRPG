using System.Collections.Generic;
using App.Common.GameItem.External.Config.Interfaces;
using App.Common.GameItem.External.Dto;
using App.Common.GameItem.Runtime.Config;
using App.Common.GameItem.Runtime.Config.Interfaces;
using App.Common.Utility.Runtime;

namespace App.Common.GameItem.External.Config
{
    public class GameItemsDtoToConfigConverter : IGameItemsDtoToConfigConverter
    {
        private readonly IReadOnlyList<IModuleDtoToConfigConverter> m_ModuleConverters;

        public GameItemsDtoToConfigConverter(IReadOnlyList<IModuleDtoToConfigConverter> moduleConverters)
        {
            m_ModuleConverters = moduleConverters;
        }

        public Optional<IGameItemsConfig> Convert(GameItemsDto dto)
        {
            var configs = new GameItemConfig[dto.Items.Count];
            for (int i = 0; i < dto.Items.Count; ++i)
            {
                var itemDto = dto.Items[i];
                var modules = new List<IModuleConfig>();
                for (int j = 0; j < m_ModuleConverters.Count; ++j)
                {
                    var converter = m_ModuleConverters[i];
                    var module = converter.Convert(itemDto.Modules);
                    if (module.HasValue)
                    {
                        modules.Add(module.Value);
                    }
                }
                
                configs[i] = new GameItemConfig(itemDto.Id, itemDto.Tags, modules);
            }
            
            var gameItemsConfig = new GameItemsConfig(configs);
            
            return Optional<IGameItemsConfig>.Success(gameItemsConfig);
        }
    }
}