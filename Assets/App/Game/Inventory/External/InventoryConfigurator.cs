using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Inventory.Runtime.Data;
using App.Game.Inventory.Runtime.Data.Model;
using App.Game.Update.External;
using UnityEngine;

namespace App.Game.Inventory.External
{
    [Configurator(DIContext.CoreContext)]
    public class InventoryConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<InventoryController>();
            BindSingle<OpenInventorySystem>();

            RegisterFSM<InventoryController>(FSMStage.CoreInitStage, StageOrders.Inventory);
        }
    }
    
    [Configurator(DIContext.GlobalContext)]
    public class GlobalInventoryConfigurator : Configurator
    {
        public override void Configuration()
        {
            RegisterData<InventoryData>();
        }
    }
}