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
        private Rigidbody _rigidbody;
        private Camera _camera;
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

        public void Init()
        {
            if (!_moveModuleSystem.TryGetModule(_context.ModuleItem, out var moveModule))
            {
                HLogger.LogError("Move not found.");
                return;
            }

            _moveModule = moveModule;
            
            _moveInput = _inputService.Input.Movement.Move;

            _camera = Camera.main;

            _rigidbody = _context.View.GetComponent<Rigidbody>();

            _animationPresenter = new MoveAnimationPresenter(_rigidbody.transform);
            _animationPresenter.Initialize();
        }

        public void OnUpdate()
        {
            var direction = _moveInput.ReadValue<Vector2>().normalized;
            _moveModule.SetDirection(new Common.Algorithms.Runtime.Vector2(direction.x, direction.y));

            var velocity2 = _moveModule.GetVelocity();
            var velocity = new Vector3(velocity2.X, 0, velocity2.Y);

            _animationPresenter.OnUpdate(velocity, Time.deltaTime);
            
            Vector3 forward = _camera.transform.forward;
            Vector3 right = _camera.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            velocity = forward * velocity.z + right * velocity.x;

            if (_context.AttackContext.IsAttack)
            {
                velocity *= 0.1f;
            }
            
            _rigidbody.linearVelocity = velocity;
            
            if (velocity != Vector3.zero && !_context.AttackContext.IsAttack)
            {
                _rigidbody.transform.forward = velocity.normalized;
            }
        }
    }
}