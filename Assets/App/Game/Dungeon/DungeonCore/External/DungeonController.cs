using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.External;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Containers.ContainerWindow.Runtime;
using App.Game.Dungeon.DungeonCore.External.Controllers;
using App.Game.Dungeon.DungeonCore.Runtime;
using App.Game.Dungeon.DungeonCore.Runtime.Services;
using App.Game.Dungeon.DungeonCreator.Runtime;
using App.Game.FollowIcon.External;
using App.Game.Inventory.External;
using UnityEngine;
using Vector2 = App.Common.Algorithms.Runtime.Vector2;

namespace App.Game.Dungeon.DungeonCore.External
{
    public class DungeonController : IDungeonController, IInitSystem
    {
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly IContainerWindowController _containerWindow;
        private readonly InventoryController _inventoryController;
        private readonly IDungeonCreator _dungeonCreator;
        private readonly IItemSpriteLoader _spriteLoader;
        private readonly IFollowIconController _followIconController;
        private readonly IAssetManager _assetManager;

        private List<RoomController> _rooms;
        private DungeonService _service;
        
        public DungeonController(
            IDungeonCreator dungeonCreator, 
            IItemSpriteLoader spriteLoader,
            IContainerWindowController containerWindow, 
            InventoryController inventoryController,
            IModuleItemsManager moduleItemsManager, 
            IFollowIconController followIconController, 
            IAssetManager assetManager)
        {
            _dungeonCreator = dungeonCreator;
            _spriteLoader = spriteLoader;
            _containerWindow = containerWindow;
            _inventoryController = inventoryController;
            _moduleItemsManager = moduleItemsManager;
            _followIconController = followIconController;
            _assetManager = assetManager;
        }

        public void Init()
        {
            if (!CreateDungeon())
            {
                return;
            }
        }

        private bool CreateDungeon()
        {
            if (!CreateService())
            {
                return false;
            }

            if (!CreateControllers())
            {
                return false;
            }
            
            return true;
        }

        private bool CreateControllers()
        {
            var dungeon = new GameObject($"Dungeon"); 
            _rooms = new List<RoomController>(_service.Rooms.Count);
            foreach (var room in _service.Rooms)
            {
                var controller = new RoomController(
                    room, 
                    dungeon,
                    _spriteLoader, 
                    _containerWindow, 
                    _inventoryController,
                    _moduleItemsManager,
                    _followIconController,
                    _assetManager);
                _rooms.Add(controller);
            }

            foreach (var room in _rooms)
            {
                room.Initialize();
            }

            return true;
        }

        private bool CreateService()
        {
            var dungeon = _dungeonCreator.Create();
            if (!dungeon.HasValue)
            {
                HLogger.LogError("Cant create dungeon");
                return false;
            }

            _service = new DungeonService(dungeon.Value);
            _service.Initialize();

            return true;
        }

        public Optional<Vector2> GetSpawnPoint()
        {
            if (_service == null)
            {
                HLogger.LogError($"Dungeon service is not initialized.");
                return Optional<Vector2>.Fail();
            }

            return Optional<Vector2>.Success(_service.GetSpawnPoint());
        }
    }
}