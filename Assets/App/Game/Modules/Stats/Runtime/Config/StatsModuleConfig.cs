using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Modules.Stats.Runtime.Config
{
    public class StatsModuleConfig : IModuleConfig
    {
        private readonly int _strength;
        private readonly int _agility;
        private readonly int _intelligence;

        public int Strength => _strength;
        public int Agility => _agility;
        public int Intelligence => _intelligence;

        public StatsModuleConfig(int strength, int agility, int intelligence)
        {
            _strength = strength;
            _agility = agility;
            _intelligence = intelligence;
        }
    }
}