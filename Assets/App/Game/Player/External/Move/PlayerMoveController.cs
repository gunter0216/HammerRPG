using App.Common.Input.Runtime;
using App.Common.Logger.Runtime;
using App.Game.Modules.Move.Runtime;
using App.Game.Player.External.Context;
using Game.Project.Gameplay.Move.External.View;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.Game.Player.External
{
    public class PlayerMoveController
    {
        private readonly MoveModuleSystem _moveModuleSystem;
        private readonly IInputService _inputService;
        private readonly PlayerContext _context;

        private MoveModule _moveModule;
        private InputAction _moveInput;
        private Rigidbody2D _rigidbody;
        private MoveAnimationPresenter _animationPresenter;

        public PlayerMoveController(
            MoveModuleSystem moveModuleSystem, 
            IInputService inputService, 
            PlayerContext context)
        {
            _moveModuleSystem = moveModuleSystem;
            _inputService = inputService;
            _context = context;
        }

        public bool Init()
        {
            if (!_moveModuleSystem.TryGetModule(_context.ModuleItem, out var moveModule))
            {
                HLogger.LogError("Move not found.");
                return false;
            }

            if (!_context.View.TryGetComponent(out _rigidbody))
            {
                HLogger.LogError("Player Rigidbody2D not found.");
                return false;
            }

            _moveModule = moveModule;
            
            _moveInput = _inputService.Input.Movement.Move;

            _animationPresenter = new MoveAnimationPresenter(_rigidbody.transform, _context.MoveContext);
            _animationPresenter.Initialize();
            return true;
        }

        public void OnUpdate()
        {
            if (_animationPresenter == null || _rigidbody == null)
                return;

            var direction = _moveInput.ReadValue<Vector2>().normalized;
            _moveModule.SetDirection(new Common.Algorithms.Runtime.Vector2(direction.x, direction.y));

            var velocity2 = _moveModule.GetVelocity();
            var velocity = new Vector2(velocity2.X, velocity2.Y);

            if (_context.AttackContext.IsAttack)
            {
                velocity *= 0.1f;
            }
            
            _rigidbody.linearVelocity = velocity;
            _animationPresenter.OnUpdate(velocity, _context.AttackContext.IsAttack);
        }
    }
}
