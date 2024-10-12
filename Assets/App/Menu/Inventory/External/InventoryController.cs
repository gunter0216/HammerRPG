using App.Common.Data.Runtime;
using App.Common.HammerDI.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Game;
using App.Game.Contexts;
using App.Menu.Inventory.Runtime.Data;

namespace App.Menu.Inventory.External
{
    [Scoped(typeof(MenuSceneContext))]
    public class InventoryController : IInitSystem
    {
        [Inject] private IDataManager m_DataManager;
        
        public void Init()
        {
             var data = m_DataManager.GetData(nameof(InventoryData)).Value as InventoryData;
             if (data == null)
             {
                 HLogger.LogError("data is null");
                 return;
             }
             
             HLogger.LogError($"{data.CountSlots}");
             data.CountSlots = 99;
        }
    }
}