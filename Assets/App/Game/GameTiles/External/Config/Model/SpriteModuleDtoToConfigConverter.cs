using System;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using App.Game.GameTiles.External.Config.Dto;

namespace App.Game.GameTiles.External.Config.Model
{
    [Singleton]
    public class SpriteModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string m_ModuleKey = "icon";
        
        public Optional<IModuleConfig> Convert(object module)
        {
            if (module is not SpriteModuleDto dto)
            {
                return Optional<IModuleConfig>.Fail();
            }
            
            var config = new SpriteModuleConfig(dto);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return m_ModuleKey;
        }

        public Type GetModuleDtoType()
        {
            return typeof(SpriteModuleDto);
        }
    }
}