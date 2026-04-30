using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Model;
using Newtonsoft.Json.Linq;

namespace Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Converter
{
    public class GameItemTypeModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string m_ModuleKey = "game_item_type";
        
        public Optional<IModuleConfig> Convert(JObject module)
        {
            var type = module.Value<string>("type");
            var config = new GameItemTypeModuleConfig(type);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return m_ModuleKey;
        }
    }
}