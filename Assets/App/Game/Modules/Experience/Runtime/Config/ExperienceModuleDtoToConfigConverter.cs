using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.Modules.Experience.Runtime.Config
{
    public class ExperienceModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "experience";
        
        public Optional<IModuleConfig> Convert(Dictionary<string, string> module)
        {
            var config = new ExperienceModuleConfig();
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}