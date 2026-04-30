using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using Newtonsoft.Json.Linq;

namespace App.Game.Modules.Move.Runtime.Config
{
    public class MoveModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "move";
        
        public Optional<IModuleConfig> Convert(JObject module)
        {
            var speed = module.Value<float>("speed");
            var config = new MoveModuleConfig(speed);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}