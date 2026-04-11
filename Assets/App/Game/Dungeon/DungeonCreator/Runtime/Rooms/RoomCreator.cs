using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Tiles;
using App.Game.Modules.Chests.Runtime;
using App.Game.Modules.ContainerModule.Runtime;
using App.Game.Modules.Doors.Runtime;
using App.Game.Modules.TilePosition.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Rooms
{
    public class RoomCreator
    {
        private readonly ChestModuleSystem _chestModuleSystem;
        private readonly ContainerModuleSystem _containerModuleSystem;
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly DoorModuleSystem _doorModuleSystem;
        private readonly KeyCreator _keyCreator;

        public RoomCreator(
            IModuleItemsManager moduleItemsManager,
            ChestModuleSystem chestModuleSystem, 
            ContainerModuleSystem containerModuleSystem, 
            DoorModuleSystem doorModuleSystem, 
            KeyCreator keyCreator)
        {
            _moduleItemsManager = moduleItemsManager;
            _chestModuleSystem = chestModuleSystem;
            _containerModuleSystem = containerModuleSystem;
            _doorModuleSystem = doorModuleSystem;
            _keyCreator = keyCreator;
        }

        public Optional<Room> CreateRoom(DungeonGenerationRoom generationRoom)
        {
            var data = new RoomData(generationRoom);
            var room = new Room(data);

            CreateWalls(room, generationRoom);
            var chestRoomCreator = new ChestRoomCreator(
                _moduleItemsManager, 
                _chestModuleSystem,
                _containerModuleSystem,
                _keyCreator);
            chestRoomCreator.CreateChests(room, generationRoom);
            var doorRoomCreator = new DoorRoomCreator(
                _moduleItemsManager,
                _doorModuleSystem,
                _keyCreator);
            doorRoomCreator.CreateDoors(room, generationRoom);
            CreateFloors(room, generationRoom);
            
            return Optional<Room>.Success(room);
        }

        private void CreateWalls(Room room, DungeonGenerationRoom generationRoom)
        {
            room.Tiles = new List<Tile>(64);
            foreach (var roomTile in generationRoom.Tiles)
            {
                if (roomTile.Value.Id != DungeonTile.Wall)
                {
                    continue;
                }
                
                var position = roomTile.Key;
                var dungeonTile = roomTile.Value.Id;
                
                var tile = CreateTile(roomTile.Value, position);
                if (!tile.HasValue)
                {
                    continue;
                }

                tile.Value.Data.Position = position;
                room.Tiles.Add(tile.Value);
                room.Data.Tiles.Add(tile.Value.Data);
            }
        }

        private void CreateFloors(Room room, DungeonGenerationRoom generationRoom)
        {
            foreach (var floor in generationRoom.Floors)
            {
                room.Data.Floors.Add(floor);    
            }
        }

        private Optional<Tile> CreateTile(
            GeneraitonTile generationTile,
            Vector2Int localPosition)
        {
            if (generationTile.Id == DungeonTile.Empty)
            {
                return Optional<Tile>.Fail();
            }
            
            var tileModuleItem = _moduleItemsManager.Create("wall");
            if (!tileModuleItem.HasValue)
            {
                HLogger.LogError($"Cant create tile");
                return Optional<Tile>.Fail();
            }

            tileModuleItem.Value.AddDataModule(new TilePositionModuleData(localPosition));

            var data = new TileData();
            data.Reference = tileModuleItem.Value.ReferenceSelf;
            var tile = new Tile(data, tileModuleItem.Value);
            
            return Optional<Tile>.Success(tile);
        }
    }
}