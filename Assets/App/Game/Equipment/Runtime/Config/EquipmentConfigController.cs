using App.Common.Configs.Runtime;
using App.Common.Logger.Runtime;
using App.Game.Equipment.Runtime.Config.Dto;
using App.Game.Equipment.Runtime.Config.Model;
using App.Game.Equipment.Runtime.Config.Converter;
using App.Game.Equipment.Runtime.Config.Loader;

namespace App.Game.Equipment.Runtime.Config
{
    public class EquipmentConfigController
    {
        private readonly IConfigLoader m_ConfigLoader;
        private EquipmentConfig m_Config;

        public EquipmentConfigController(IConfigLoader configLoader)
        {
            m_ConfigLoader = configLoader;
        }

        public bool Initialize()
        {
            var configLoader = new EquipmentConfigLoader(m_ConfigLoader);
            var dtoOptional = configLoader.Load();
            if (!dtoOptional.HasValue)
            {
                HLogger.LogError("EquipmentConfig is null");
                return false;
            }

            var converter = new EquipmentConfigConverter();
            var config = converter.Convert(dtoOptional.Value);
            if (config == null)
            {
                HLogger.LogError("EquipmentConfig conversion failed");
                return false;
            }

            m_Config = config;
            return true;
        }

        public EquipmentConfig GetConfig()
        {
            return m_Config;
        }
    }
}
