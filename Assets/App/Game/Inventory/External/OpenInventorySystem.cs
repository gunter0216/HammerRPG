using App.Common.Input.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Inventory.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.Game.Inventory.External
{
    public class OpenInventorySystem
    {
        private readonly IInventoryController _inventoryController;
        private readonly IInputService _inputService;

        public OpenInventorySystem(IInventoryController inventoryController, IInputService inputService)
        {
            _inventoryController = inventoryController;
            _inputService = inputService;

            _inputService.Input.UI.Inventory.performed += OnOpenClick;
        }

        private void OnOpenClick(InputAction.CallbackContext obj)
        {
            if (_inventoryController.IsOpen())
            {
                _inventoryController.CloseWindow();
            }
            else
            {
                _inventoryController.OpenWindow();
            }
        }
    }
}