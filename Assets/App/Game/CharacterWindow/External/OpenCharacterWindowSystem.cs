using App.Common.Input.Runtime;
using UnityEngine.InputSystem;

namespace App.Game.CharacterWindow.External
{
    public class OpenCharacterWindowSystem
    {
        private readonly CharacterWindowController _characterWindowController;
        private readonly IInputService _inputService;

        public OpenCharacterWindowSystem(CharacterWindowController characterWindowController,
            IInputService inputService)
        {
            _characterWindowController = characterWindowController;
            _inputService = inputService;

            _inputService.Input.UI.Character.performed += OnClick;
        }

        private void OnClick(InputAction.CallbackContext obj)
        {
            if (_characterWindowController.IsOpen())
            {
                _characterWindowController.Close();
            }
            else
            {
                _characterWindowController.Open();
            }
        }
    }
}