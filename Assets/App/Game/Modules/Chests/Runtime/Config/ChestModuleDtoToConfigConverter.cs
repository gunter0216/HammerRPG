using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.Modules.Chests.Runtime.Config
{
    public class ChestModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "chest";
        
        public Optional<IModuleConfig> Convert(Dictionary<string, string> module)
        {
            var closeIconKey = module["close_icon_key"];
            var openIconKey = module["open_icon_key"];
            var emptyIconKey = module["empty_icon_key"];
            var config = new ChestModuleConfig(closeIconKey, openIconKey, emptyIconKey);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}