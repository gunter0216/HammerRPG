using App.Common.Logger.Runtime;
using App.Game.Player.External.Context;
using App.Game.Player.External.Items;
using App.Game.Player.External.View;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.Game.Player.External.Attack
{
    public class PlayerAttackController
    {
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int AttackFacingLeft = Animator.StringToHash("AttackFacingLeft");

        private readonly PlayerContext _context;
        private readonly PlayerAttackContext _attackContext;
        private Animator _animator;
        private MeleeAttackService _attackService;
        private bool _facingLeft;
        private bool _repeatAttack;

        public PlayerAttackController(PlayerContext context)
        {
            _context = context;
            _attackContext = context.AttackContext;
        }

        public bool Initialize()
        {
            var view = _context.View;
            _animator = view.Animator;
            var attackView = view.GetComponent<MeleeAttackView>();
            var listener = attackView != null ? attackView.AttackAnimationEventListener : null;
            if (_animator == null || _animator.runtimeAnimatorController == null || listener == null)
            {
                HLogger.LogError("Player melee animation or attack event listener is missing.");
                return false;
            }

            _attackService = new MeleeAttackService(_context.ModuleItem, attackView, view);
            listener.OnAttackEvent += OnAttackEvent;
            listener.OnAttackFinishedEvent += OnAttackFinished;
            return true;
        }

        public void OnAttackClick()
        {
            if (_attackContext.IsAttack || _attackContext.AttackBlocked || !_animator.isActiveAndEnabled ||
                _animator.GetFloat(HandItemsController.WeaponEquipped) < 0.5f)
                return;

            _facingLeft = GetAttackDirection();
            _context.MoveContext.IsFacingLeft = _facingLeft;
            _attackContext.IsAttack = true;
            _repeatAttack = false;
            _animator.SetBool(AttackFacingLeft, _facingLeft);
            _animator.SetTrigger(Attack);
        }

        public void OnUpdate()
        {
            if (!_repeatAttack)
                return;

            _repeatAttack = false;
            if (Mouse.current?.leftButton.isPressed == true)
                OnAttackClick();
        }

        private bool GetAttackDirection()
        {
            var camera = Camera.main;
            var mouse = Mouse.current;
            if (camera == null || mouse == null)
                return _context.MoveContext.IsFacingLeft;

            var player = camera.WorldToScreenPoint(_context.View.transform.position);
            float offset = mouse.position.ReadValue().x - player.x;
            return Mathf.Abs(offset) > 1f ? offset < 0f : _context.MoveContext.IsFacingLeft;
        }

        private void OnAttackEvent()
        {
            if (_attackContext.IsAttack)
                _attackService.OnAttackEvent(_facingLeft);
        }

        private void OnAttackFinished()
        {
            if (!_attackContext.IsAttack)
                return;

            _attackContext.IsAttack = false;
            _repeatAttack = true;
        }
    }
}
