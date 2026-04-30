using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Modules.Race.Runtime.Config
{
    public class RaceModuleConfig : IModuleConfig
    {
        private readonly string _name;

        public string Race => _name;

        public RaceModuleConfig(string race)
        {
            _name = race;
        }
    }
}