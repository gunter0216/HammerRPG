using System.Collections.Generic;
using App.Common.ModuleItem.External.Config.Interfaces;
using App.Common.ModuleItem.External.Dto;
using App.Common.ModuleItem.Runtime.Config;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utility.Runtime;

namespace App.Common.ModuleItem.External.Config
{
    public class ModuleItemsDtoToConfigConverter : IModuleItemsDtoToConfigConverter
    {
        private readonly IReadOnlyList<IModuleDtoToConfigConverter> m_ModuleConverters;

        public ModuleItemsDtoToConfigConverter(IReadOnlyList<IModuleDtoToConfigConverter> moduleConverters)
        {
            m_ModuleConverters = moduleConverters;
        }

        public Optional<IModuleItemsConfig> Convert(ModuleItemsDto dto, string type)
        {
            var configs = new ModuleItemConfig[dto.Items.Count];
            for (int i = 0; i < dto.Items.Count; ++i)
            {
                var itemDto = dto.Items[i];
                var modules = new List<IModuleConfig>();
                if (itemDto.Modules != null)
                {
                    for (int j = 0; j < m_ModuleConverters.Count; ++j)
                    {
                        var converter = m_ModuleConverters[j];
                        var module = converter.Convert(itemDto.Modules);
                        if (module.HasValue)
                        {
                            modules.Add(module.Value);
                        }
                    }
                }

                configs[i] = new ModuleItemConfig(itemDto.Id, itemDto.Tags, modules, type);
            }
            
            var gameItemsConfig = new ModuleItemsConfig(configs);
            
            return Optional<IModuleItemsConfig>.Success(gameItemsConfig);
        }
    }
}