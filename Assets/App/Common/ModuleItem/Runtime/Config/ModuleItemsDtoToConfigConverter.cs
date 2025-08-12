using System.Collections.Generic;
using App.Common.Data.Runtime.Deserializer;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using Assets.App.Common.ModuleItem.Runtime.Config.Dto;
using Assets.App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace Assets.App.Common.ModuleItem.Runtime.Config
{
    public class ModuleItemsDtoToConfigConverter : IModuleItemsDtoToConfigConverter
    {
        private readonly ILogger m_Logger;
        private readonly Dictionary<string, IModuleDtoToConfigConverter> m_ModuleConverters;
        private readonly IJsonDeserializer m_JsonDeserializer;

        public ModuleItemsDtoToConfigConverter(
            IJsonDeserializer jsonDeserializer,
            ILogger logger,
            IReadOnlyList<IModuleDtoToConfigConverter> moduleConverters)
        {
            m_JsonDeserializer = jsonDeserializer;
            m_Logger = logger;
            m_ModuleConverters = new Dictionary<string, IModuleDtoToConfigConverter>(moduleConverters.Count);
            foreach (var converter in moduleConverters)
            {
                var key = converter.GetModuleKey();
                m_ModuleConverters.Add(key, converter);
            }
        }

        public Optional<IModuleItemsConfig> Convert(ModuleItemsDto dto, string type)
        {
            var configs = new ModuleItemConfig[dto.Items.Count];
            for (int i = 0; i < dto.Items.Count; ++i)
            {
                var itemDto = dto.Items[i];
                var modules = CreateModules(itemDto);

                configs[i] = new ModuleItemConfig(itemDto.Id, itemDto.Tags, modules, type);
            }

            var gameItemsConfig = new ModuleItemsConfig(configs);

            return Optional<IModuleItemsConfig>.Success(gameItemsConfig);
        }

        private IReadOnlyList<IModuleConfig> CreateModules(ModuleItemDto itemDto)
        {
            var modules = new List<IModuleConfig>();
            if (itemDto.Modules == null)
            {
                return modules;
            }

            foreach (var moduleDto in itemDto.Modules)
            {
                var key = moduleDto.Key;
                var content = moduleDto.Content;
                if (m_ModuleConverters.TryGetValue(key, out var converter))
                {
                    var moduleDtoType = converter.GetModuleDtoType();
                    var dto = m_JsonDeserializer.Deserialize(content, moduleDtoType);
                    if (!dto.HasValue)
                    {
                        m_Logger.LogError("[ModuleItemsDtoToConfigConverter] Failed to deserialize module DTO with key: " + key);
                        continue;
                    }
                    
                    var module = converter.Convert(dto.Value);
                    if (!module.HasValue)
                    {
                        m_Logger.LogError("[ModuleItemsDtoToConfigConverter] Failed to convert module DTO with key: " + key);
                        continue;
                    }
                        
                    modules.Add(module.Value);
                }
            }

            return modules;
        }
    }
}