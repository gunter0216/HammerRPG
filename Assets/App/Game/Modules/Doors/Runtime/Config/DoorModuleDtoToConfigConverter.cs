using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.Modules.Doors.Runtime.Config
{
    public class DoorModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "door";
        
        public Optional<IModuleConfig> Convert(Dictionary<string, string> module)
        {
            var closeIconKey = module["close_icon_key"];
            var openIconKey = module["open_icon_key"];
            var config = new DoorModuleConfig(closeIconKey, openIconKey);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}