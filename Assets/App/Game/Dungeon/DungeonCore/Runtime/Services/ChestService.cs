using System;
using App.Common.ModuleItem.Runtime;
using App.Game.Containers.ContainerWindow.Runtime;
using App.Game.Modules.Chests.Runtime;
using App.Game.Modules.ContainerModule.Runtime;

namespace App.Game.Dungeon.DungeonCore.Runtime.Services
{
    public class ChestService
    {
        private readonly IContainerWindowController _containerWindow;
        private readonly IModuleItemsManager _moduleItemsManager;
        
        private IModuleItem _item;
        private ChestModule _chestModule;
        private ContainerModule _containerModule;

        public ChestModule Module => _chestModule;

        public ChestService(IModuleItemsManager moduleItemsManager, IContainerWindowController containerWindow)
        {
            _moduleItemsManager = moduleItemsManager;
            _containerWindow = containerWindow;
        }

        public void Initialize()
        {
            var item = _moduleItemsManager.Create("chest");
            _item = item.Value;
            _chestModule = _item.GetModule<ChestModule>().Value;
            _containerModule = _item.GetModule<ContainerModule>().Value;
        }

        public void Open(Action onClosedCallback)
        {
            _containerWindow.OpenWindow(_containerModule.Container, onClosedCallback);
            _chestModule.SetUsedState();
        }
    }
}