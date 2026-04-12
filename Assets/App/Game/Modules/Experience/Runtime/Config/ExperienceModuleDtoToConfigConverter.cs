using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using Newtonsoft.Json.Linq;

namespace App.Game.Modules.Experience.Runtime.Config
{
    public class ExperienceModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "experience";
        
        public Optional<IModuleConfig> Convert(JObject module)
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