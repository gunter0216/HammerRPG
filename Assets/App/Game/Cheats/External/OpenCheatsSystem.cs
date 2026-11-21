using System;
using App.Common.Input.Runtime;
using UnityEngine.InputSystem;

namespace App.Game.Cheats.External
{
    public class OpenCheatsSystem : IDisposable
    {
        private readonly CheatsController _cheatsController;
        private readonly IInputService _inputService;

        public OpenCheatsSystem(CheatsController cheatsController, IInputService inputService)
        {
            _cheatsController = cheatsController;
            _inputService = inputService;
            _inputService.Input.UI.Cheats.performed += OnClick;
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            if (_cheatsController.IsOpen())
                _cheatsController.CloseWindow();
            else
                _cheatsController.OpenWindow();
        }

        public void Dispose()
        {
            _inputService.Input.UI.Cheats.performed -= OnClick;
        }
    }
}
