using System;
using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using App.Game.ModuleItemType.Runtime.Config.Model;

namespace App.Game.ModuleItemType.Runtime.Config.Converter
{
    public class EquipmentModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string m_ModuleKey = "equipment";
        
        public Optional<IModuleConfig> Convert(Dictionary<string, string> module)
        {
            var type = module["type"];
            var config = new EquipmentModuleConfig(type);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return m_ModuleKey;
        }
    }
}