using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;

namespace App.Game.Modules.Health.Runtime.Config
{
    public class HealthModuleConfig : ModuleConfig
    {
        [SerializeField] private float _maxHealth;

        public float MaxHealth => _maxHealth;

        public HealthModuleConfig(float maxHealth)
        {
            _maxHealth = maxHealth;
        }
    }
}