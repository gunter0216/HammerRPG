using App.Common.Configs.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime.Config.Dto;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.External
{
    public class ModuleItemsConfigLoader : IInitSystem
    {
        private readonly (string configKey, string groupKey)[] _configs = new[]
        {
            ("GameModuleItemsConfig", ModuleItemType: ModuleItemConfigs.GameItemsType),
            ("TileItemsConfig", ModuleItemType: ModuleItemConfigs.GameTilesType),
            ("EntitiesConfig", ModuleItemType: ModuleItemConfigs.EntitiesType),
        };
        
        private readonly IConfigLoader _configLoader;
        private readonly ModuleItemsManager _moduleItemsManager;

        public ModuleItemsConfigLoader(
            IConfigLoader configLoader, 
            ModuleItemsManager moduleItemsManager)
        {
            _configLoader = configLoader;
            _moduleItemsManager = moduleItemsManager;
        }

        public void Init()
        {
            foreach (var (configKey, groupKey) in _configs)
            {
                var dto =  _configLoader.LoadConfig<ModuleItemsDto>(configKey);
                if (!dto.HasValue)
                {
                    HLogger.LogError($"Cant load config {configKey}");
                    return;
                }
                
                _moduleItemsManager.RegisterItems(dto.Value, groupKey);
            }
        }
    }
}