using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Modules.Name.Runtime.Config
{
    public class NameModuleConfig : ModuleConfig
    {
        private readonly string _name;

        public string Name => _name;

        public NameModuleConfig(string name)
        {
            _name = name;
        }
    }
}