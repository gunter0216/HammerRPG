using App.Common.Input.Runtime;
using App.Common.Windows.External;
using App.Game.Interactions.External;
using App.Game.Player.External.Attack;
using App.Game.Player.External.Context;
using UnityEngine.InputSystem;

namespace App.Game.Player.External
{
    public class LeftClickController
    {
        private readonly IInputService _inputService;
        private readonly IWindowManager _windowManager;
        private readonly PlayerContext _context;
        private readonly PlayerAttackController _playerAttackController;

        private readonly InteractionController _interactionController;

        public LeftClickController(IInputService inputService,
            IWindowManager windowManager,
            PlayerContext context,
            PlayerAttackController playerAttackController,
            InteractionController interactionController)
        {
            _inputService = inputService;
            _windowManager = windowManager;
            _context = context;
            _playerAttackController = playerAttackController;
            _interactionController = interactionController;
        }

        public void Initialize()
        {
            _interactionController.Initialize();
            _inputService.Input.UI.LeftClick.performed += OnLeftClick;
        }

        private void OnLeftClick(InputAction.CallbackContext obj)
        {
            if (_windowManager.IsAnyOpen() || _interactionController.OnLeftClick())
            {
                return;
            }
            
            _playerAttackController.OnAttackClick();
        }

        public void OnUpdate()
        {
            _context.AttackContext.AttackBlocked = _windowManager.IsAnyOpen();
            
            _interactionController.OnUpdate(!_context.AttackContext.AttackBlocked);
            _playerAttackController.OnUpdate();
        }
    }
}
