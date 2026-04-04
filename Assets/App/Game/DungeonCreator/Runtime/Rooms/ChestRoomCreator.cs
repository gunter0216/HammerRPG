using System.Collections.Generic;
using App.Common.DataContainer.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.GameTiles.External.Config.Data;
using App.Generation.DungeonCreator.Runtime.Chest;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Chest;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Game.DungeonCreator.Runtime.Rooms
{
    public class ChestRoomCreator
    {
        private const string _chestKey = "chest";

        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly ChestModuleSystem _chestModuleSystem;
        private readonly ContainerModuleSystem _containerModuleSystem;

        public ChestRoomCreator(
            IModuleItemsManager moduleItemsManager,
            ChestModuleSystem chestModuleSystem,
            ContainerModuleSystem containerModuleSystem)
        {
            _moduleItemsManager = moduleItemsManager;
            _chestModuleSystem = chestModuleSystem;
            _containerModuleSystem = containerModuleSystem;
        }

        public void CreateChests(Room room, DungeonGenerationRoom generationRoom)
        {
            var generationChests = generationRoom.Chests;
            var chests = new List<Chest>(generationChests.Count);
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

        private Chest CreateChest(
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

            var chest = new Chest(
                chestItem.Value,
                room,
                tilePosition,
                chestModule,
                containerModule);

            var key = CreateKey(generationChest.Key);
            if (key != null)
            {
                containerModule.AddItem(key);
            }

            return chest;
        }

        private IModuleItem CreateKey(DungeonKeyData dungeonKey)
        {
            if (dungeonKey == null)
            {
                HLogger.LogError("Key is empty");
                return null;
            }

            var moduleItem = _moduleItemsManager.Create("IronKey");
            if (!moduleItem.HasValue)
            {
                HLogger.LogError($"Cant create moduleItem");
                return null;
            }

            var keyModuleData = new KeyModuleData(dungeonKey);
            if (!moduleItem.Value.AddDataModule(keyModuleData))
            {
                HLogger.LogError("Cant add key data");
                return null;
            }

            return moduleItem.Value;
        }
    }
}