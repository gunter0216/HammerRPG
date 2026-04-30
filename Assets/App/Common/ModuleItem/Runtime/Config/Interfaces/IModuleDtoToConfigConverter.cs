using App.Common.Utilities.Utility.Runtime;
using Newtonsoft.Json.Linq;

namespace App.Common.ModuleItem.Runtime.Config.Interfaces
{
    public interface IModuleDtoToConfigConverter
    {
        Optional<IModuleConfig> Convert(JObject module);
        string GetModuleKey();
    }
}