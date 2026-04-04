using System;
using System.Collections.Generic;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace App.Game.Modules.ContainerModule.Runtime.Config
{
    public class ContainerModuleDtoToConfigConverter : IModuleDtoToConfigConverter
    {
        private const string m_ModuleKey = "container";
        
        public Optional<IModuleConfig> Convert(Dictionary<string, string> module)
        {
            var rows = Int32.Parse(module["rows"]);
            var cols = Int32.Parse(module["cols"]);
            var config = new ContainerModuleConfig(rows: rows, cols: cols);
            
            return Optional<IModuleConfig>.Success(config);
        }

        public string GetModuleKey()
        {
            return m_ModuleKey;
        }
    }
}