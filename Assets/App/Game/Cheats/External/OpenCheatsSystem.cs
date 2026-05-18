using App.Common.Input.Runtime;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.Game.Cheats.External
{
    public class OpenCheatsSystem
    {
        private readonly CheatsController m_CheatsController;
        private readonly IInputService _inputService;

        public OpenCheatsSystem(CheatsController cheatsController, IInputService inputService)
        {
            m_CheatsController = cheatsController;
            _inputService = inputService;

            _inputService.Input.UI.Cheats.performed += OnClick;
        }

        private void OnClick(InputAction.CallbackContext obj)
        {
            if (m_CheatsController.IsOpen())
            {
                m_CheatsController.CloseWindow();
            }
            else
            {
                m_CheatsController.OpenWindow();
            }
        }
    }
}