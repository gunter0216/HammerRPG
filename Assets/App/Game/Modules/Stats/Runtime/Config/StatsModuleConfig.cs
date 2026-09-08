using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;

namespace App.Game.Modules.Stats.Runtime.Config
{
    public class StatsModuleConfig : ModuleConfig
    {
        [SerializeField] private int _strength;
        [SerializeField] private int _agility;
        [SerializeField] private int _intelligence;

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