using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Doors;
using App.Game.Modules.Doors.Runtime;
using App.Game.Modules.TilePosition.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Door;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Rooms
{
    public class DoorRoomCreator
    {
        private const string _doorKey = "door";

        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly DoorModuleSystem _doorModuleSystem;
        private readonly KeyCreator _keyCreator;

        public DoorRoomCreator(
            IModuleItemsManager moduleItemsManager, 
            DoorModuleSystem doorModuleSystem, 
            KeyCreator keyCreator)
        {
            _moduleItemsManager = moduleItemsManager;
            _doorModuleSystem = doorModuleSystem;
            _keyCreator = keyCreator;
        }

        public void CreateDoors(Room room, DungeonGenerationRoom generationRoom)
        {
            var generationRoomDoors = generationRoom.Doors;
            var doors = new List<Door>(generationRoomDoors.Count);
            room.Doors = doors;

            foreach (var generationDoor in generationRoomDoors)
            {
                var door = CreateDoor(room, generationDoor);
                if (door == null)
                {
                    continue;
                }
                
                doors.Add(door);
            }
        }

        private Door CreateDoor(
            Room room,
            GenerationDoor generationDoor)
        {
            var doorItem = _moduleItemsManager.Create(_doorKey);
            if (!doorItem.HasValue)
            {
                HLogger.LogError("Cant create Door.");
                return null;
            }

            if (!_doorModuleSystem.TryGetModule(doorItem.Value, out var doorModule))
            {
                HLogger.LogError("Cant get module.");
                return null;
            }
        
            var tilePosition = new TilePositionModuleData(generationDoor.LocalPosition);
            doorItem.Value.AddDataModule(tilePosition);
        
            var door = new Door(
                doorItem.Value,
                room,
                tilePosition,
                doorModule);

            if (generationDoor.RequiredKey != null)
            {
                var key = _keyCreator.Create(generationDoor.RequiredKey);
                if (key == null)
                {
                    HLogger.LogError("Cant create key.");
                }
                else
                {
                    doorModule.SetKey(key.ReferenceSelf);
                    doorModule.Close();
                }
            }

            return door;
        }
    }
}