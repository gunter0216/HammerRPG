using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.Modules.Stats.Runtime.Config
{
    public class StatsModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "stats";
        
        public Optional<IModuleConfig> Convert(Dictionary<string, string> module)
        {
            var strength = module["strength"];
            var agility = module["agility"];
            var intelligence = module["intelligence"];
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