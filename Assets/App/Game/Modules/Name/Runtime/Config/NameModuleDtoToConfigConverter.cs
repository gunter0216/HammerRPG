using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using Newtonsoft.Json.Linq;

namespace App.Game.Modules.Name.Runtime.Config
{
    public class NameModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "name";
        
        public Optional<IModuleConfig> Convert(JObject module)
        {
            var name = module.Value<string>("name");
            var config = new NameModuleConfig(name);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}