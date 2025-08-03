using App.Common.Autumn.Runtime.Attributes;
using App.Game.Contexts;
using App.Game.Update.Runtime;
using App.Game.Update.Runtime.Attributes;
using App.Menu.Inventory.External;
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
                m_InventoryController.OpenWindow();
            }
        }
    }
}