using System.Collections.Generic;
using App.Common.ModuleItem.External;
using App.Common.ModuleItem.Runtime;
using App.Game.Equipment.Runtime.Config.Model;
using App.Game.Modules.Health.Runtime;
using App.Game.Player.External.View;
using Game.Project.Gameplay.Weapon.Runtime.DamageHandlers;
using UnityEngine;

namespace App.Game.Player.External.Attack
{
    internal sealed class MeleeAttack2D
    {
        private readonly IModuleItem _attacker;
        private readonly MeleeAttackView _view;
        private readonly List<Collider2D> _overlaps = new();
        private readonly List<RaycastHit2D> _lineHits = new();
        private readonly Dictionary<IModuleItem, IDamageHandler> _targets = new();

        public MeleeAttack2D(IModuleItem attacker, MeleeAttackView view)
        {
            _attacker = attacker;
            _view = view;
        }

        public bool CanHit(OneHandedWeaponModuleConfig weapon, bool facingLeft, IModuleItem target)
        {
            if (target == null)
                return false;

            CollectTargets(weapon, facingLeft, target);
            return _targets.ContainsKey(target);
        }

        public void Strike(OneHandedWeaponModuleConfig weapon, bool facingLeft, IModuleItem onlyTarget = null)
        {
            Physics2D.SyncTransforms();
            CollectTargets(weapon, facingLeft, onlyTarget);
            if (_targets.Count == 0)
                return;

            var damage = new HitModel(_attacker, weapon.Damage);
            foreach (var handler in _targets.Values)
                handler.Handle(damage);
        }

        private void CollectTargets(OneHandedWeaponModuleConfig weapon, bool facingLeft, IModuleItem onlyTarget)
        {
            _targets.Clear();
            var (origin, center, size, angle) = _view.GetSwingArea2D(weapon, facingLeft);
            if (size.x <= 0 || size.y <= 0 || weapon.Damage <= 0)
                return;

            var scene = _view.gameObject.scene.GetPhysicsScene2D();
            var targetFilter = new ContactFilter2D { useTriggers = true };
            targetFilter.SetLayerMask(_view.EnemyLayer);
            scene.OverlapBox(center, size, angle, targetFilter, _overlaps);

            var obstacleFilter = new ContactFilter2D { useTriggers = false };
            obstacleFilter.SetLayerMask(_view.SwingObstacleLayers);
            var forward = (center - origin).normalized;
            foreach (var collider in _overlaps)
            {
                if (collider.transform.IsChildOf(_view.transform) ||
                    Vector2.Dot((Vector2)collider.bounds.center - origin, forward) <= 0)
                    continue;

                var handler = collider.GetComponentInParent<IDamageHandler>();
                var target = handler?.ModuleItem;
                if (target == null || target == _attacker || (onlyTarget != null && target != onlyTarget) ||
                    _targets.ContainsKey(target) ||
                    (target.TryGetModule<HealthModule>(out var health) && health.IsDie))
                    continue;

                var hitPoint = collider.ClosestPoint(origin);
                if (!IsBlocked(scene, origin, hitPoint, obstacleFilter, collider, target))
                    _targets.Add(target, handler);
            }
        }

        private bool IsBlocked(PhysicsScene2D scene, Vector2 origin, Vector2 hitPoint, ContactFilter2D filter,
            Collider2D targetCollider, IModuleItem target)
        {
            scene.Linecast(origin, hitPoint, filter, _lineHits);
            foreach (var hit in _lineHits)
            {
                var blocker = hit.collider;
                if (blocker.transform.IsChildOf(_view.transform) ||
                    blocker == targetCollider ||
                    blocker.GetComponentInParent<ModuleItemView>()?.ModuleItem == target)
                    continue;

                return true;
            }

            return false;
        }
    }
}
