using System;
using App.Common.ModuleItem.Runtime;
using App.Game.Modules.Health.Runtime.Config;
using App.Game.Modules.Health.Runtime.Data;
using UnityEngine;

namespace App.Game.Modules.Health.Runtime
{
    public class HealthModule : IModule
    {
        private readonly IModuleItem _item;
        private readonly HealthModuleData _data;
        private readonly HealthModuleConfig _config;

        private Action _onHealthChanged;
        private Action _onHealthOver;

        public Action OnHealthChanged
        {
            get => _onHealthChanged;
            set => _onHealthChanged = value;
        }
        
        public Action OnHealthOver
        {
            get => _onHealthOver;
            set => _onHealthOver = value;
        }

        public bool IsDie => _data.Health <= 0;
        public float Health => _data.Health;
        public float MaxHealth => _config.MaxHealth;

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
            if (_data.Health <= 0)
            {
                return;
            }
            
            _data.Health -= health;
            if (Mathf.Approximately(_data.Health, 0))
            {
                _data.Health = 0;
                _onHealthOver?.Invoke();
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