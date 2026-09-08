// using System.Collections.Generic;
// using App.Common.ModuleItem.Runtime.Config.Interfaces;
// using App.Common.Utilities.Utility.Runtime;
// using Newtonsoft.Json.Linq;
//
// namespace App.Game.Modules.Sprite.Runtime
// {
//     public class SpriteModuleDtoToConfigConverter : IModuleDtoToConfigConverter
//     {
//         private const string m_ModuleKey = "icon";
//         
//         public Optional<IModuleConfig> Convert(JObject module)
//         {
//             var iconKey = module.Value<string>("icon_key");
//             var config = new SpriteModuleConfig(iconKey);
//             
//             return Optional<IModuleConfig>.Success(config);
//         }
//
//         public string GetModuleKey()
//         {
//             return m_ModuleKey;
//         }
//     }
// }