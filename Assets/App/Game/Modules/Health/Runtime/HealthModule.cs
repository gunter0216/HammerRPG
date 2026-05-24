using System;
using App.Common.ModuleItem.Runtime;
using App.Game.Modules.Health.Runtime.Config;
using App.Game.Modules.Health.Runtime.Data;

namespace App.Game.Modules.Health.Runtime
{
    public class HealthModule : IModule
    {
        private readonly IModuleItem _item;
        private readonly HealthModuleData _data;
        private readonly HealthModuleConfig _config;

        private Action _onHealthChanged;

        public Action OnHealthChanged
        {
            get => _onHealthChanged;
            set => _onHealthChanged = value;
        }

        public HealthModuleData Data => _data;
        public HealthModuleConfig Config => _config;

        public HealthModule(
            IModuleItem item, 
            HealthModuleData data, 
            HealthModuleConfig config)
        {
            _item = item;
            _data = data;
            _config = config;
        }

        public void Spend(float health)
        {
            _data.Health -= health;
            if (_data.Health < 0)
            {
                _data.Health = 0;
            }
            
            _onHealthChanged?.Invoke();
        }

        public void Add(float health)
        {
            _data.Health += health;
            if (_data.Health > _config.MaxHealth)
            {
                _data.Health = _config.MaxHealth;
            }
            
            _onHealthChanged?.Invoke();
        }
    }
}