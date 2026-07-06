using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Modules.Health.Runtime.Config
{
    public class HealthModuleConfig : ModuleConfig
    {
        private readonly float _maxHealth;

        public float MaxHealth => _maxHealth;

        public HealthModuleConfig(float maxHealth)
        {
            _maxHealth = maxHealth;
        }
    }
}