using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using Newtonsoft.Json.Linq;

namespace App.Game.Modules.Level.Runtime.Config
{
    public class LevelModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "level";
        
        public Optional<IModuleConfig> Convert(JObject module)
        {
            var level = module.Value<int>("start_level");
            var config = new LevelModuleConfig(level);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}