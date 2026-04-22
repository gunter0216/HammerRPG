using App.Common.Input.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Modules.Move.Runtime;
using App.Game.Player.External.View;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.Game.Player.External
{
    public class PlayerMoveController
    {
        private readonly MoveModuleSystem _moveModuleSystem;
        private readonly IInputService _inputService;
        private readonly IModuleItem _player;
        private readonly EntityView _view;

        private MoveModule _moveModule;
        private InputAction _moveInput;
        private Rigidbody _rigidbody;
        private Camera _camera;

        public PlayerMoveController(
            MoveModuleSystem moveModuleSystem,
            IInputService inputService,
            IModuleItem player,
            EntityView view)
        {
            _moveModuleSystem = moveModuleSystem;
            _inputService = inputService;
            _player = player;
            _view = view;
        }

        public void Init()
        {
            if (!_moveModuleSystem.TryGetModule(_player, out var moveModule))
            {
                HLogger.LogError("Move not found.");
                return;
            }

            _moveModule = moveModule;
            
            _moveInput = _inputService.Input.Movement.Move;

            _camera = Camera.main;

            _rigidbody = _view.GetComponent<Rigidbody>();
        }

        public void OnUpdate()
        {
            var direction = _moveInput.ReadValue<Vector2>().normalized;
            _moveModule.SetDirection(new Common.Algorithms.Runtime.Vector2(direction.x, direction.y));

            var velocity2 = _moveModule.GetVelocity();
            var velocity = new Vector3(velocity2.X, 0, velocity2.Y);
            
            Vector3 forward = _camera.transform.forward;
            Vector3 right = _camera.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            velocity = forward * velocity.z + right * velocity.x;
            
            _rigidbody.velocity = velocity;
        }
    }
}