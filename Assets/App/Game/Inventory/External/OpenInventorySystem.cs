using App.Common.Utilities.Utility.Runtime;
using App.Game.Inventory.Runtime;
using UnityEngine;

namespace App.Game.Inventory.External
{
    public class OpenInventorySystem : IUpdateSystem
    {
        private readonly IInventoryController _inventoryController;

        public OpenInventorySystem(IInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        public void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
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
}