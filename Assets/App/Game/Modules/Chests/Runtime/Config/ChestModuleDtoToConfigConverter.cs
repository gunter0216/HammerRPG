using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using Newtonsoft.Json.Linq;

namespace App.Game.Modules.Chests.Runtime.Config
{
    public class ChestModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "chest";
        
        public Optional<IModuleConfig> Convert(JObject module)
        {
            var closeIconKey = module.Value<string>("close_icon_key");
            var openIconKey = module.Value<string>("open_icon_key");
            var emptyIconKey = module.Value<string>("empty_icon_key");
            var config = new ChestModuleConfig(closeIconKey, openIconKey, emptyIconKey);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}