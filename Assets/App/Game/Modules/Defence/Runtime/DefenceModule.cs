using System;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Modules.Health.Runtime;
using Game.Project.Gameplay.Weapon.Runtime.DamageHandlers;

namespace App.Game.Modules.Defence.Runtime
{
    public class DefenceModule : IModule
    {
        private readonly IModuleItem _moduleItem;
        private readonly HealthModule _healthModule;

        public DefenceModule(IModuleItem moduleItem, HealthModule healthModule)
        {
            _moduleItem = moduleItem;
            _healthModule = healthModule;
        }

        public void OnDamage(HitModel model, IDamageHandler handler)
        {
            var target = handler.ModuleItem;
            if (target == model.SourceModel)
            {
                return;
            }   
            
            _healthModule.Spend(model.Damage);
        }
    }
}