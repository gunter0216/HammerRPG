using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Modules.Stats.Runtime.Config
{
    public class StatsModuleConfig : IModuleConfig
    {
        private readonly string _strength;
        private readonly string _agility;
        private readonly string _intelligence;

        public string Strength => _strength;
        public string Agility => _agility;
        public string Intelligence => _intelligence;

        public StatsModuleConfig(string strength, string agility, string intelligence)
        {
            _strength = strength;
            _agility = agility;
            _intelligence = intelligence;
        }
    }
}