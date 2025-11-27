using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Game.Inventory.External
{
    public class OpenInventorySystem : IRunSystem
    {
        private readonly IInventoryController m_InventoryController;

        public OpenInventorySystem(IInventoryController inventoryController)
        {
            m_InventoryController = inventoryController;
        }

        public void Run()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (m_InventoryController.IsOpen())
                {
                    m_InventoryController.CloseWindow();
                }
                else
                {
                    m_InventoryController.OpenWindow();   
                }
            }
        }
    }
}