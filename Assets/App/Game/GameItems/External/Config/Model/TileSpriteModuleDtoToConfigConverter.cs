using App.Common.Autumn.Runtime.Attributes;
using App.Common.ModuleItem.External.Config.Interfaces;
using App.Common.ModuleItem.External.Dto;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utility.Runtime;

namespace App.Game.GameTiles.External.Config.Model
{
    [Singleton]
    public class GameItemTypeModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        public Optional<IModuleConfig> Convert(ModuleItemModulesDto modules)
        {
            if (modules.GameItemType == null)
            {
                return Optional<IModuleConfig>.Fail();
            }
            
            var module = new GameItemTypeModuleConfig(modules.GameItemType.Type);
            
            return Optional<IModuleConfig>.Success(module);
        }
    }
}