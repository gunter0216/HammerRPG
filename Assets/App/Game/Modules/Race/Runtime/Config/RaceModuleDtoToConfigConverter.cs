using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using Newtonsoft.Json.Linq;

namespace App.Game.Modules.Race.Runtime.Config
{
    public class RaceModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "race";
        
        public Optional<IModuleConfig> Convert(JObject module)
        {
            var race = module.Value<string>("race");
            var config = new RaceModuleConfig(race);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}