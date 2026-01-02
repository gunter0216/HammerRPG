using App.Common.Utilities.Utility.Runtime;
using App.Game.Equipment.Runtime;
using UnityEngine;

namespace App.Game.Inventory.External
{
    public class OpenInventorySystem : IRunSystem
    {
        private readonly IInventoryController m_InventoryController;
        private readonly IEquipmentController m_EquipmentController;

        public OpenInventorySystem(IInventoryController inventoryController, IEquipmentController equipmentController)
        {
            m_InventoryController = inventoryController;
            m_EquipmentController = equipmentController;
        }

        public void Run()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (m_InventoryController.IsOpen())
                {
                    m_InventoryController.CloseWindow();
                    m_EquipmentController.CloseWindow();
                }
                else
                {
                    m_InventoryController.OpenWindow();   
                    m_EquipmentController.OpenWindow();   
                }
            }
        }
    }
}