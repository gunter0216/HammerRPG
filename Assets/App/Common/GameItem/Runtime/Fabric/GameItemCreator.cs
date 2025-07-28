using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.GameItem.Runtime.Config.Interfaces;
using App.Common.GameItem.Runtime.Data;
using App.Common.GameItem.Runtime.Fabric.Interfaces;
using App.Common.GameItem.Runtime.Services;
using App.Common.Utility.Runtime;

namespace App.Common.GameItem.Runtime.Fabric
{
    public class GameItemCreator : IGameItemCreator
    {
        private readonly IGameItemConfigController m_ConfigController;
        private readonly IContainersDataManager m_ContainerController;
        private readonly IReadOnlyList<ICreateGameItemHandler> m_Handlers;

        public GameItemCreator(
            IGameItemConfigController configController, 
            IContainersDataManager containerController, 
            IReadOnlyList<ICreateGameItemHandler> handlers)
        {
            m_ConfigController = configController;
            m_ContainerController = containerController;
            m_Handlers = handlers;
        }

        public Optional<IGameItem> Create(string id)
        {
            var dataReferences = new List<DataReference>();
            var data = new GameItemData(id, dataReferences);
            return Create(data);
        }

        public Optional<IGameItem> Create(IGameItemData data)
        {
            var config = m_ConfigController.GetConfig(data.Id);
            if (!config.HasValue)
            {
                return Optional<IGameItem>.Fail();
            }

            var modulesHolder = new ModulesHolder(m_ContainerController, data.ModuleRefs);
            IGameItem gameItem = new GameItem(modulesHolder, config.Value, data);
            foreach (var handler in m_Handlers)
            {
                var handledGameItem = handler.Handle(gameItem);
                if (!handledGameItem.HasValue)
                {
                    return Optional<IGameItem>.Fail();
                }

                gameItem = handledGameItem.Value;
            }
            
            return Optional<IGameItem>.Success(gameItem);
        }
    }
}