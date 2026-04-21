using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using Newtonsoft.Json.Linq;

namespace App.Game.Modules.Health.Runtime.Config
{
    public class HealthModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "health";
        
        public Optional<IModuleConfig> Convert(JObject module)
        {
            var health = module.Value<float>("health");
            var config = new HealthModuleConfig(health);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}