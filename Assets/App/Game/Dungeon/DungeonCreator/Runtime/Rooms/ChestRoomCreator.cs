using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Chest;
using App.Game.Modules.Chests.Runtime;
using App.Game.Modules.ContainerModule.Runtime;
using App.Game.Modules.TilePosition.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Chest;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Rooms
{
    public class ChestRoomCreator
    {
        private const string _chestKey = "chest";

        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly ChestModuleSystem _chestModuleSystem;
        private readonly ContainerModuleSystem _containerModuleSystem;
        private readonly KeyCreator _keyCreator;

        public ChestRoomCreator(
            IModuleItemsManager moduleItemsManager,
            ChestModuleSystem chestModuleSystem,
            ContainerModuleSystem containerModuleSystem, 
            KeyCreator keyCreator)
        {
            _moduleItemsManager = moduleItemsManager;
            _chestModuleSystem = chestModuleSystem;
            _containerModuleSystem = containerModuleSystem;
            _keyCreator = keyCreator;
        }

        public void CreateChests(Room room, DungeonGenerationRoom generationRoom)
        {
            var generationChests = generationRoom.Chests;
            var chests = new List<Chest.Chest>(generationChests.Count);
            room.Chests = chests;

            foreach (var generationChest in generationChests)
            {
                var chest = CreateChest(room, generationChest);
                if (chest == null)
                {
                    continue;
                }
                
                chests.Add(chest);
            }
        }

        private Chest.Chest CreateChest(
            Room room,
            DungeonGenerationChest generationChest)
        {
            var localPosition = room.GetLocalCenter();

            var chestItem = _moduleItemsManager.Create(_chestKey);
            if (!chestItem.HasValue)
            {
                HLogger.LogError("Cant create chest.");
                return null;
            }

            if (!_chestModuleSystem.TryGetModule(chestItem.Value, out var chestModule))
            {
                HLogger.LogError("Chest module not found.");
                return null;
            }

            if (!_containerModuleSystem.TryGetModule(chestItem.Value, out var containerModule))
            {
                HLogger.LogError("Chest module not found.");
                return null;
            }

            var tilePosition = new TilePositionModuleData(localPosition);
            chestItem.Value.AddDataModule(tilePosition);

            var chest = new Chest.Chest(
                chestItem.Value,
                room,
                tilePosition,
                chestModule,
                containerModule);

            var key = _keyCreator.Create(generationChest.Key);
            if (key != null)
            {
                containerModule.AddItem(key);
            }

            return chest;
        }
    }
}