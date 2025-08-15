using System.Collections.Generic;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.Data.Runtime.Deserializer;
using App.Common.DataContainer.Runtime;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.ModuleItem.Runtime.Config;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.ModuleItem.Runtime.Fabric;
using App.Common.ModuleItem.Runtime.Fabric.Interfaces;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Contexts;
using App.Game.States.Runtime.Game;

namespace App.Common.ModuleItem.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), -100_000)]
    public class ModuleItemsManager : IInitSystem, IModuleItemsManager
    {
        [Inject] private readonly IContainersDataManager m_ContainersDataManager;
        [Inject] private readonly List<IModuleDtoToConfigConverter> m_ModuleDtoToConfigConverters;
        [Inject] private readonly List<ICreateModuleItemHandler> m_Handlers;
        [Inject] private readonly IJsonDeserializer m_JsonDeserializer;
        [Inject] private readonly ILogger m_Logger;
        
        private ModuleItemsConfigController m_ConfigController;
        private ModuleItemCreator m_ModuleItemCreator;

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