using App.Common.ModuleItem.Runtime;
using App.Game.Equipment.Runtime.Config;
using App.Game.Equipment.Runtime.Data;

namespace App.Game.Equipment.External
{
    public class EquipmentItemsController
    {
        private readonly EquipmentConfigController m_ConfigController;
        private readonly IEquipmentDataController m_DataController;
        private readonly IModuleItemsManager m_ModuleItemsManager;
        
        public EquipmentItemsController(
            EquipmentConfigController configController, 
            IEquipmentDataController dataController, 
            IModuleItemsManager moduleItemsManager)
        {
            m_ConfigController = configController;
            m_DataController = dataController;
            m_ModuleItemsManager = moduleItemsManager;
        }

        public bool Initialize()
        {
            return true;
        }
    }
}