using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.Modules.Name.Runtime.Config
{
    public class NameModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "name";
        
        public Optional<IModuleConfig> Convert(Dictionary<string, string> module)
        {
            var name = module["name"];
            var config = new NameModuleConfig(name);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}