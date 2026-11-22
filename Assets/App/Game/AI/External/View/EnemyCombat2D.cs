using System.Collections.Generic;
using App.Common.ModuleItem.External;
using App.Common.ModuleItem.Runtime;
using App.Game.Equipment.Runtime.Config.Model;
using App.Game.Modules.Health.Runtime;
using App.Game.Modules.Move.Runtime;
using App.Game.Player.External.Attack;
using App.Game.Player.External.View;
using UnityEngine;

namespace App.Game.AI.External.View
{
    internal sealed class EnemyCombat2D
    {
        private const float DetectionRadius = 5f;
        private static readonly int FacingLeft = Animator.StringToHash("FacingLeft");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Death = Animator.StringToHash("Die");

        private readonly IModuleItem _item;
        private readonly EntityView _view;
        private readonly Rigidbody2D _body;
        private readonly MoveModule _move;
        private readonly List<Collider2D> _nearby = new();
        private Animator _animator;
        private AttackAnimationEventListener _listener;
        private OneHandedWeaponModuleConfig _weapon;
        private MeleeAttack2D _melee;
        private ModuleItemView _target;
        private IModuleItem _swingTarget;
        private bool _facingLeft;
        private float _nextScan;

        public EnemyCombat2D(IModuleItem item, EntityView view, Rigidbody2D body, MoveModule move)
        {
            _item = item;
            _view = view;
            _body = body;
            _move = move;
        }

        public bool Initialize()
        {
            _animator = _view.Animator;
            if (_animator == null || _animator.runtimeAnimatorController == null ||
                !_view.TryGetComponent<MeleeAttackView>(out var attackView) ||
                !_item.TryGetConfigModule<OneHandedWeaponModuleConfig>(out _weapon))
                return false;

            _listener = attackView.AttackAnimationEventListener;
            if (_listener == null)
                return false;

            _facingLeft = _animator.GetBool(FacingLeft);
            _melee = new MeleeAttack2D(_item, attackView);
            _listener.OnAttackEvent += OnContact;
            _listener.OnAttackFinishedEvent += OnAttackFinished;
            return true;
        }

        public void Update()
        {
            if (_swingTarget != null)
                return;

            if (!IsInRange(_target))
                _target = FindTarget();
            if (_target == null)
            {
                SetMovement(Vector2.zero);
                return;
            }

            Vector2 delta = (Vector2)_target.transform.position - _body.position;
            if (Mathf.Abs(delta.x) > 0.01f)
                _facingLeft = delta.x < 0;
            _animator.SetBool(FacingLeft, _facingLeft);

            if (_melee.CanHit(_weapon, _facingLeft, _target.ModuleItem))
            {
                SetMovement(Vector2.zero);
                _swingTarget = _target.ModuleItem;
                _animator.SetTrigger(Attack);
                return;
            }

            if (Mathf.Abs(delta.x) < 0.25f && Mathf.Abs(delta.y) < 0.75f)
                delta.x += _facingLeft ? 0.5f : -0.5f;
            SetMovement(delta.normalized);
        }

        public void Die()
        {
            _swingTarget = null;
            _listener.OnAttackEvent -= OnContact;
            _listener.OnAttackFinishedEvent -= OnAttackFinished;
            SetMovement(Vector2.zero);
            _body.simulated = false;
            _animator.ResetTrigger(Attack);
            _animator.SetBool(Death, true);
        }

        private ModuleItemView FindTarget()
        {
            if (Time.time < _nextScan)
                return null;

            _nextScan = Time.time + 0.2f;
            var scene = _view.gameObject.scene.GetPhysicsScene2D();
            var filter = new ContactFilter2D { useTriggers = false };
            scene.OverlapCircle(_body.position, DetectionRadius, filter, _nearby);
            foreach (var collider in _nearby)
            {
                var candidate = collider.GetComponentInParent<ModuleItemView>();
                if (candidate?.ModuleItem?.Id == "player" && IsInRange(candidate))
                    return candidate;
            }

            return null;
        }

        private bool IsInRange(ModuleItemView target)
        {
            return target != null && target.ModuleItem != null &&
                ((Vector2)target.transform.position - _body.position).sqrMagnitude <= DetectionRadius * DetectionRadius &&
                target.ModuleItem.TryGetModule<HealthModule>(out var health) && !health.IsDie;
        }

        private void SetMovement(Vector2 direction)
        {
            _move.SetDirection(new Common.Algorithms.Runtime.Vector2(direction.x, direction.y));
            var velocity = _move.GetVelocity();
            var movement = new Vector2(velocity.X, velocity.Y);
            _body.linearVelocity = movement;
            _animator.SetBool(Run, movement.sqrMagnitude > 0.0001f);
        }

        private void OnContact()
        {
            if (_swingTarget != null)
                _melee.Strike(_weapon, _facingLeft, _swingTarget);
        }

        private void OnAttackFinished() => _swingTarget = null;
    }
}
