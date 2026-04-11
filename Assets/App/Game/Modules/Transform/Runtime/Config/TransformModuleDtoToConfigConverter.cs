using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.Modules.Transform.Runtime.Config
{
    public class TransformModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "transform";
        
        public Optional<IModuleConfig> Convert(Dictionary<string, string> module)
        {
            var config = new TransformModuleConfig();
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}