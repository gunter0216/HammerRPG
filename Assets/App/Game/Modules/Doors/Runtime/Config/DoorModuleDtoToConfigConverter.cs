using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using Newtonsoft.Json.Linq;

namespace App.Game.Modules.Doors.Runtime.Config
{
    public class DoorModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "door";
        
        public Optional<IModuleConfig> Convert(JObject module)
        {
            var config = new DoorModuleConfig();
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}