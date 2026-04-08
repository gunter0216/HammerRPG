using App.Common.ModuleItem.Runtime.Config.Interfaces;

namespace App.Game.Modules.Health.Runtime.Config
{
    public class HealthModuleConfig : IModuleConfig
    {
        private readonly string _maxHealth;

        public string MaxHealth => _maxHealth;

        public HealthModuleConfig(string maxHealth)
        {
            _maxHealth = maxHealth;
        }
    }
}