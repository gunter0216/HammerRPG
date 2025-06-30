using App.Common.Autumn.Runtime.Attributes;
using App.Common.Data.Runtime;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Game;
using App.Game.Contexts;
using App.Game.Inventory.External.Data;
using App.Game.States.Menu;

namespace App.Menu.Inventory.External
{
    [Scoped(typeof(MenuSceneContext))]
    [Stage(typeof(MenuInitPhase), 0)]
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
             
             // HLogger.LogError($"{data.CountSlots}");
             // data.CountSlots = 99;
        }
    }
}