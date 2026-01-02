using System;
using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using App.Game.ModuleItemType.Runtime.Config.Dto;
using App.Game.ModuleItemType.Runtime.Config.Model;

namespace App.Game.ModuleItemType.Runtime.Config.Converter
{
    public class GameItemTypeModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string m_ModuleKey = "game_item_type";
        
        public Optional<IModuleConfig> Convert(Dictionary<string, string> module)
        {
            var type = module["type"];
            var config = new GameItemTypeModuleConfig(type);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return m_ModuleKey;
        }

        public Type GetModuleDtoType()
        {
            return typeof(GameItemTypeModuleDto);
        }
    }
}