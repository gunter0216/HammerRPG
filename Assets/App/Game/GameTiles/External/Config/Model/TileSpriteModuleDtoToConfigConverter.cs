using System;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.ModuleItem.External.Dto;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utility.Runtime;

namespace App.Game.GameTiles.External.Config.Model
{
    [Singleton]
    public class TileSpriteModuleDtoToConfigConverter : ITileModuleDtoToConfigConverter
    {
        public Optional<IModuleConfig> Convert(ModuleItemModulesDto modules)
        {
            if (modules.TileSpriteModule == null)
            {
                return Optional<IModuleConfig>.Fail();
            }
            
            var module = new TileSpriteModuleConfig(modules.TileSpriteModule.Key);
            
            return Optional<IModuleConfig>.Success(module);
        }
    }
}