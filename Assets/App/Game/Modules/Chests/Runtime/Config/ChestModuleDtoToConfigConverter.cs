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
            var config = new ChestModuleConfig();
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}