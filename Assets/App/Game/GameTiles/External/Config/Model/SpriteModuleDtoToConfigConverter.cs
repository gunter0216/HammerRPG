using System;
using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using App.Game.GameTiles.External.Config.Dto;

namespace App.Game.GameTiles.External.Config.Model
{
    public class SpriteModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string m_ModuleKey = "icon";
        
        public Optional<IModuleConfig> Convert(Dictionary<string, string> module)
        {
            var iconKey = module["icon_key"];
            var config = new SpriteModuleConfig(iconKey);
            
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