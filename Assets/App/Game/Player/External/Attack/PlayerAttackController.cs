using App.Game.Player.External.Context;
using App.Game.Player.External.View;
using DG.Tweening;
using Game.Project.Gameplay.Weapon.Runtime.DamageHandlers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.Game.Player.External.Attack
{
    public class PlayerAttackController
    {
        private static readonly int _attack = Animator.StringToHash("Attack");
        private static readonly int _melee = Animator.StringToHash("Melee");

        private readonly PlayerContext _context;
        private readonly PlayerAttackContext _attackContext;

        private Camera _camera;
        private MeleeAttackView _meleeAttackView;

        public PlayerAttackController(PlayerContext context)
        {
            _context = context;
            _attackContext = _context.AttackContext;
        }

        public void Initialize()
        {
            _camera = Camera.main;

            var view = _context.View;
            _meleeAttackView = view.GetComponent<MeleeAttackView>();
            var animator = view.Animator;
            animator.SetBool(_melee, true);
            _meleeAttackView.AttackAnimationEventListener.OnAttackEvent += OnAttackEvent;
        }

        public void OnAttackClick()
        {
            if (_attackContext.IsAttack || IsBlocked())
            {
                return;
            }
            
            Attack();
        }

        private void Attack()
        {
            _attackContext.IsAttack = true;

            var view = _context.View;
            var animator = view.Animator;

            animator.SetBool(_attack, true);

            DOTween.Sequence()
                .AppendInterval(_meleeAttackView.AttackAnimation.length)
                .OnComplete(() =>
                {
                    if (IsLeftMousePressed() && !IsBlocked())
                    {
                        Attack();
                        return;
                    }
                    
                    _attackContext.IsAttack = false;
                    animator.SetBool(_attack, false);
                });
        }

        public void OnUpdate()
        {
            if (_attackContext.IsAttack && IsLeftMousePressed() && !IsBlocked())
            {
                RotatePlayerByDirection();
            }
        }

        private bool IsBlocked()
        {
            return _attackContext.AttackBlocked;
        }

        private bool IsLeftMousePressed()
        {
            return Mouse.current.leftButton.isPressed;
        }

        private void OnAttackEvent()
        {
            var view = _context.View;
            Vector3 center = _meleeAttackView.GetAttackCenter();

            Collider[] hits = Physics.OverlapBox(
                center,
                _meleeAttackView.BoxSize / 2f,
                _context.View.transform.rotation,
                _meleeAttackView.EnemyLayer);

            foreach (var hit in hits)
            {
                if (!hit.TryGetComponent<IDamageHandler>(out var handler))
                {
                    continue;
                }

                var hitModel = new HitModel(_context.ModuleItem, 10);
                handler.Handle(hitModel);
            }
        }

        private void RotatePlayerByDirection()
        {
            var transform = _context.View.transform;

            var ray = _camera.ScreenPointToRay(
                Mouse.current.position.ReadValue());

            Plane plane = new Plane(Vector3.up, transform.position);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);

                Vector3 direction = hitPoint - transform.position;
                direction.y = 0f;

                if (direction.sqrMagnitude > 0.001f)
                {
                    transform.rotation =
                        Quaternion.LookRotation(direction);
                }
            }
        }
    }
}