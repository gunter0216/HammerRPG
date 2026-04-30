using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using Newtonsoft.Json.Linq;

namespace App.Game.Modules.Stats.Runtime.Config
{
    public class StatsModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "stats";
        
        public Optional<IModuleConfig> Convert(JObject module)
        {
            var strength = module.Value<int>("strength");
            var agility = module.Value<int>("agility");
            var intelligence = module.Value<int>("intelligence");
            var config = new StatsModuleConfig(
                strength,
                agility,
                intelligence);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}