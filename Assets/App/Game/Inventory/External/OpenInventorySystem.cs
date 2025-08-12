using App.Common.Autumn.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Game.Contexts;
using App.Game.Update.Runtime;
using App.Game.Update.Runtime.Attributes;
using UnityEngine;

namespace App.Game.Inventory.External
{
    [Scoped(typeof(GameSceneContext))]
    [RunSystem(-1000)]
    public class OpenInventorySystem : IRunSystem
    {
        [Inject] private readonly IInventoryController m_InventoryController;
        
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