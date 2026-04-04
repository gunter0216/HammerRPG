using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.GameTiles.Runtime;
using App.Game.Modules.Chest.Runtime;
using App.Game.Modules.ContainerModule.Runtime;
using App.Game.Modules.Door.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Rooms
{
    public class RoomsCreator
    {
        private readonly ChestModuleSystem _chestModuleSystem;
        private readonly ContainerModuleSystem _containerModuleSystem;
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly ITilesController _tilesController;
        private readonly RoomCreator _roomCreator;
        private readonly DoorModuleSystem _doorModuleSystem;
        private readonly KeyCreator _keyCreator;

        public RoomsCreator(ITilesController tilesController,
            IModuleItemsManager moduleItemsManager,
            ChestModuleSystem chestModuleSystem, 
            ContainerModuleSystem containerModuleSystem, DoorModuleSystem doorModuleSystem)
        {
            _tilesController = tilesController;
            _moduleItemsManager = moduleItemsManager;
            _chestModuleSystem = chestModuleSystem;
            _containerModuleSystem = containerModuleSystem;
            _doorModuleSystem = doorModuleSystem;
            _keyCreator = new KeyCreator(_moduleItemsManager);
            _roomCreator = new RoomCreator(
                _tilesController,
                _moduleItemsManager,
                _chestModuleSystem, 
                _containerModuleSystem,
                _doorModuleSystem,
                _keyCreator);
        }

        public void CreateRooms(DungeonGeneration generation, Dungeon dungeon)
        {
            var dataGenerationRooms = generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var startRoom = dataGenerationRooms.StartGenerationRoom;
            var endRoom = dataGenerationRooms.EndGenerationRoom;
            var generationRooms = dataGenerationRooms.Rooms;

            var rooms = new List<Room>(generationRooms.Count);

            foreach (var generationRoom in generationRooms)
            {
                if (generationRoom.RequiredKey == null)
                {
                    continue;
                }
                
                _keyCreator.Create(generationRoom.RequiredKey);
            }

            foreach (var generationRoom in generationRooms)
            {
                var room = _roomCreator.CreateRoom(generationRoom);
                if (!room.HasValue)
                {
                    HLogger.LogError("cant create room");
                    return;
                }
                
                rooms.Add(room.Value);

                if (startRoom == generationRoom)
                {
                    dungeon.StartRoom = room.Value;
                    dungeon.Data.StartRoom = room.Value.Data.UID;
                } 
                else if (endRoom == generationRoom)
                {
                    dungeon.EndRoom = room.Value;
                    dungeon.Data.EndRoom = room.Value.Data.UID;
                }
            }

            dungeon.Data.Rooms = new List<RoomData>(rooms.Count);
            foreach (var room in rooms)
            {
                dungeon.Data.Rooms.Add(room.Data);
            }

            dungeon.Rooms = rooms;
        }
    }
}