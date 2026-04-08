using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.Modules.Race.Runtime.Config
{
    public class RaceModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "race";
        
        public Optional<IModuleConfig> Convert(Dictionary<string, string> module)
        {
            var race = module["race"];
            var config = new RaceModuleConfig(race);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}