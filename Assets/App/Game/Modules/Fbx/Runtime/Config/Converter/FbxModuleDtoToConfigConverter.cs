using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Model;
using Newtonsoft.Json.Linq;

namespace Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Converter
{
    public class FbxModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string _moduleKey = "fbx";
        
        public Optional<IModuleConfig> Convert(JObject module)
        {
            var type = module.Value<string>("asset_key");
            var config = new FbxModuleConfig(type);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return _moduleKey;
        }
    }
}