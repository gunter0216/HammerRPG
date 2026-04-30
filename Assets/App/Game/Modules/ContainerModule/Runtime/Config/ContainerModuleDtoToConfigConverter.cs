using System;
using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using Newtonsoft.Json.Linq;

namespace App.Game.Modules.ContainerModule.Runtime.Config
{
    public class ContainerModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string m_ModuleKey = "container";
        
        public Optional<IModuleConfig> Convert(JObject module)
        {
            var rows = module.Value<int>("rows");
            var cols = module.Value<int>("cols");
            var config = new ContainerModuleConfig(rows: rows, cols: cols);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return m_ModuleKey;
        }
    }
}