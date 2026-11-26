using System;
using App.Common.Input.Runtime;
using UnityEngine.InputSystem;

namespace App.Common.Windows.External
{
    public class ClosePopWindowService : IDisposable
    {
        private readonly WindowManager _windowManager;
        private readonly IInputService _inputService;

        public event Action BackUnhandled;

        public ClosePopWindowService(WindowManager windowManager, IInputService inputService)
        {
            _windowManager = windowManager;
            _inputService = inputService;

            _inputService.Input.UI.Back.performed += OnBackClick;
        }

        private void OnBackClick(InputAction.CallbackContext obj)
        {
            if (_windowManager.IsAnyOpen() && _windowManager.TryPopWindow())
                return;

            BackUnhandled?.Invoke();
        }

        public void Dispose()
        {
            _inputService.Input.UI.Back.performed -= OnBackClick;
        }
    }
}
