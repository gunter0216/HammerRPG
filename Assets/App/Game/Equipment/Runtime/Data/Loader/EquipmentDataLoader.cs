using App.Common.Data.Runtime;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Equipment.Runtime.Data;

namespace App.Game.Equipment.External.Data
{
    public class EquipmentDataLoader
    {
        private readonly IDataManager m_DataManager;

        public EquipmentDataLoader(IDataManager dataManager)
        {
            m_DataManager = dataManager;
        }

        public Optional<EquipmentData> Load()
        {
            var data = m_DataManager.GetData<EquipmentData>(nameof(EquipmentData));
            if (!data.HasValue)
            {
                HLogger.LogError("EquipmentData is null");
                return Optional<EquipmentData>.Fail();
            }
            
            return data;
        }
    }
}