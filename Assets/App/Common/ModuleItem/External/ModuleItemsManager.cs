using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.FSM.Runtime;
using App.Common.Json.Runtime.Deserializer;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.ModuleItem.Runtime.Fabric;
using App.Common.ModuleItem.Runtime.Fabric.Interfaces;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.ModuleItem.External
{
    public class ModuleItemsManager : IInitSystem, IModuleItemsManager
    {
        private readonly IContainersDataManager m_ContainersDataManager;
        private readonly List<IModuleDtoToConfigConverter> m_ModuleDtoToConfigConverters;
        private readonly List<ICreateModuleItemHandler> m_Handlers;
        private readonly IJsonDeserializer m_JsonDeserializer;
        private readonly ILogger m_Logger;
        
        private ModuleItemsConfigController m_ConfigController;
        private ModuleItemCreator m_ModuleItemCreator;

        public ModuleItemsManager(
            IContainersDataManager containersDataManager,
            List<IModuleDtoToConfigConverter> moduleDtoToConfigConverters,
            List<ICreateModuleItemHandler> handlers, 
            IJsonDeserializer jsonDeserializer,
            ILogger logger)
        {
            m_ContainersDataManager = containersDataManager;
            m_ModuleDtoToConfigConverters = moduleDtoToConfigConverters;
            m_Handlers = handlers;
            m_JsonDeserializer = jsonDeserializer;
            m_Logger = logger;
        }

        public void Init()
        {
            InitConfigController();
            InitItemsFabric();
        }

        private void InitConfigController()
        {
            m_ConfigController = new ModuleItemsConfigController();
        }

        private void InitItemsFabric()
        {
            m_ModuleItemCreator = new ModuleItemCreator(
                m_ConfigController, 
                m_ContainersDataManager, 
                m_Handlers);
        }

        public bool RegisterItems(IModuleItemsConfigLoader moduleItemsConfigLoader, string type)
        {
            var dto = moduleItemsConfigLoader.Load();
            if (!dto.HasValue)
            {
                HLogger.LogError($"[BaseModuleItemsManager] In method Init, cant load file.");
                return false;
            }

            var dtoConverter = new ModuleItemsDtoToConfigConverter(
                m_JsonDeserializer,
                m_Logger,
                m_ModuleDtoToConfigConverters);
            var config = dtoConverter.Convert(dto.Value, type);
            if (!config.HasValue)
            {
                HLogger.LogError($"[BaseModuleItemsManager] In method Init, cant convert dto to configs.");
                return false;
            }

            return RegisterItems(config.Value.Configs, type);
        }

        public bool RegisterItems(IReadOnlyList<IModuleItemConfig> configs, string type)
        {
            return m_ConfigController.RegisterItems(configs, type);
        }

        public Optional<IModuleItem> Create(DataReference dataReference)
        {
            return m_ModuleItemCreator.Create(dataReference);
        }

        public Optional<IModuleItem> Create(string id)
        {
            return m_ModuleItemCreator.Create(id);
        }

        public bool Destroy(IModuleItem data)
        {
            return false;
        }

        public Optional<IModuleItemConfig> GetConfig(string id)
        {
            return m_ConfigController.GetConfig(id);
        }

        public Optional<IReadOnlyList<IModuleItemConfig>> GetConfigs(string type)
        {
            return m_ConfigController.GetConfigs(type);
        }
    }
}