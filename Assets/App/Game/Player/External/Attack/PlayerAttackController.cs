using App.Common.Input.Runtime;
using App.Game.Player.External.Context;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.Game.Player.External.Attack
{
    public class PlayerAttackController
    {
        private static readonly int _attack = Animator.StringToHash("Attack");
        private static readonly int _melee = Animator.StringToHash("Melee");

        private readonly IInputService _inputService;
        private readonly PlayerContext _context;
        private readonly PlayerAttackContext _attackContext;

        private Camera _camera;

        public PlayerAttackController(
            IInputService inputService,
            PlayerContext context)
        {
            _inputService = inputService;
            _context = context;
            _attackContext = _context.AttackContext;
        }

        public void Initialize()
        {
            _inputService.Input.Player.Attack.performed += OnAttackClick;

            _camera = Camera.main;

            var view = _context.View;
            var animator = view.Animator;
            animator.SetBool(_melee, true);
            view.AttackAnimationEventListener.OnAttackEvent += OnAttackEvent;
        }

        private void OnAttackClick(InputAction.CallbackContext obj)
        {
            if (_attackContext.IsAttack)
            {
                return;
            }

            RotatePlayerByDirection();

            _attackContext.IsAttack = true;

            var view = _context.View;
            var animator = view.Animator;

            animator.SetBool(_attack, true);

            DOTween.Sequence()
                .AppendInterval(view.AttackAnimation.length)
                .OnComplete(() =>
                {
                    _attackContext.IsAttack = false;
                    animator.SetBool(_attack, false);
                });
        }

        private void OnAttackEvent()
        {
            var view = _context.View;
            Vector3 center = view.GetAttackCenter();

            Collider[] hits = Physics.OverlapBox(
                center,
                view.BoxSize / 2f,
                _context.View.transform.rotation,
                view.EnemyLayer);

            foreach (var hit in hits)
            {
                // if (hit.TryGetComponent<IDamageable>(out var damageable))
                // {
                //     damageable.TakeDamage(10);
                // }
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