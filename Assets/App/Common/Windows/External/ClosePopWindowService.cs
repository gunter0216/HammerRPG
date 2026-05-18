using App.Common.Input.Runtime;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.Common.Windows.External
{
    public class ClosePopWindowService
    {
        private readonly WindowManager _windowManager;
        private readonly IInputService _inputService;

        public ClosePopWindowService(WindowManager windowManager, IInputService inputService)
        {
            _windowManager = windowManager;
            _inputService = inputService;

            _inputService.Input.UI.Back.performed += OnBackClick;
            // todo otpiska
        }

        private void OnBackClick(InputAction.CallbackContext obj)
        {
            if (_windowManager.IsAnyOpen())
            {
                _windowManager.TryPopWindow();
            }
        }
    }
}