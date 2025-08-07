using System;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.ModuleItem.External.Config.Interfaces;
using App.Common.ModuleItem.External.Dto;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utility.Runtime;

namespace App.Game.GameTiles.External.Config.Model
{
    [Singleton]
    public class SpriteModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        public Optional<IModuleConfig> Convert(ModuleItemModulesDto modules)
        {
            if (modules.SpriteModule == null)
            {
                return Optional<IModuleConfig>.Fail();
            }
            
            var module = new SpriteModuleConfig(modules.SpriteModule.Key);
            
            return Optional<IModuleConfig>.Success(module);
        }
    }
}