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

        public void OnAttackEvent(bool facingLeft = false)
        {
            var handlers = GetHandlers(facingLeft);
            ProduceDamage(handlers);
        }

        public List<IDamageHandler> GetHandlers(bool facingLeft = false)
        {
            var center = _attackView.GetAttackCenter(facingLeft);

            var count = RayCastHelper.OverlapBoxNonAlloc(
                center,
                _attackView.BoxSize,
                out var hits,
                _attackView.EnemyLayer,
                _entityView.transform.rotation);

            var result = new List<IDamageHandler>(count);

            for (int i = 0; i < count; ++i)
            {
                var hit = hits[i];
                if (hit.transform.IsChildOf(_entityView.transform) || !hit.TryGetComponent<IDamageHandler>(out var handler))
                {
                    continue;
                }
                
                result.Add(handler);
            }

            return result;
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
