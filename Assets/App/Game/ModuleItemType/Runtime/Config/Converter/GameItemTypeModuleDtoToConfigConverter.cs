using System;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using App.Game.ModuleItemType.Runtime.Config.Dto;
using App.Game.ModuleItemType.Runtime.Config.Model;

namespace App.Game.ModuleItemType.Runtime.Config.Converter
{
    public class GameItemTypeModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string m_ModuleKey = "game_item_type";
        
        public Optional<IModuleConfig> Convert(object module)
        {
            if (module is not GameItemTypeModuleDto dto)
            {
                return Optional<IModuleConfig>.Fail();
            }
            
            var config = new GameItemTypeModuleConfig(dto);
            
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