using App.Common.Configs.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Equipment.Runtime.Config.Dto;

namespace App.Game.Equipment.Runtime.Config.Loader
{
    public class EquipmentConfigLoader
    {
        private const string m_LocalConfigKey = "EquipmentConfig";
        private readonly IConfigLoader m_ConfigLoader;

        public EquipmentConfigLoader(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public Optional<EquipmentConfigDto> Load()
        {
            return m_ConfigLoader.LoadConfig<EquipmentConfigDto>(m_LocalConfigKey);
        }
    }
}
