using System.Collections.Generic;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.UtilityUnity.Runtime;
using App.Game.Player.External.View;
using Game.Project.Gameplay.Weapon.Runtime.DamageHandlers;

namespace App.Game.Player.External.Attack
{
    public class MeleeAttackService
    {
        private readonly IModuleItem _moduleItem;
        private readonly MeleeAttackView _attackView;
        private readonly EntityView _entityView;

        public MeleeAttackService(IModuleItem moduleItem, MeleeAttackView attackView, EntityView entityView)
        {
            _attackView = attackView;
            _entityView = entityView;
            _moduleItem = moduleItem;
        }

        public void OnAttackEvent()
        {
            var handlers = GetHandlers();
            ProduceDamage(handlers);
        }

        public List<IDamageHandler> GetHandlers()
        {
            var center = _attackView.GetAttackCenter();

            var count = RayCastHelper.OverlapBoxNonAlloc(
                center,
                _attackView.BoxSize,
                out var hits,
                _attackView.EnemyLayer,
                _entityView.transform.rotation);

            var result = new List<IDamageHandler>(count);

            for (int i = 0; i < count; ++i)
            {
                if (!hits[i].TryGetComponent<IDamageHandler>(out var handler))
                {
                    continue;
                }
                
                result.Add(handler);
            }

            return result;
            
            foreach (var hit in hits)
            {
                if (!hit.TryGetComponent<IDamageHandler>(out var handler))
                {
                    continue;
                }
        
                var hitModel = new HitModel(_moduleItem, 10);
                handler.Handle(hitModel);
            }
        }

        public void ProduceDamage(IReadOnlyList<IDamageHandler> handlers)
        {
            foreach (var handler in handlers)
            {
                var hitModel = new HitModel(_moduleItem, 10);
                handler.Handle(hitModel);
            }
        }
    }
}