using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.Modules.Move.Runtime.Config
{
    public class MoveModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "move";
        
        public Optional<IModuleConfig> Convert(Dictionary<string, string> module)
        {
            var move = module["start_move"];
            var config = new MoveModuleConfig(move);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}