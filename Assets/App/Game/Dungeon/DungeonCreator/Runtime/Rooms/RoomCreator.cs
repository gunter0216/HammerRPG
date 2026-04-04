using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Chest.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Door;
using App.Game.Dungeon.DungeonCreator.Runtime.Tiles;
using App.Game.GameTiles.Runtime;
using App.Game.Modules.ContainerModule.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Rooms
{
    public class RoomCreator
    {
        private readonly ChestModuleSystem _chestModuleSystem;
        private readonly ContainerModuleSystem _containerModuleSystem;
        private readonly ITilesController _tilesController;
        private readonly IModuleItemsManager _moduleItemsManager;

        public RoomCreator(
            ITilesController tilesController,
            IModuleItemsManager moduleItemsManager,
            ChestModuleSystem chestModuleSystem, 
            ContainerModuleSystem containerModuleSystem)
        {
            _tilesController = tilesController;
            _moduleItemsManager = moduleItemsManager;
            _chestModuleSystem = chestModuleSystem;
            _containerModuleSystem = containerModuleSystem;
        }

        public Optional<Room> CreateRoom(DungeonGenerationRoom generationRoom)
        {
            var data = new RoomData(generationRoom);
            var room = new Room(data);

            CreateWalls(room, generationRoom);
            CreateDoors(room, generationRoom);
            var chestRoomCreator = new ChestRoomCreator(
                _moduleItemsManager, 
                _chestModuleSystem,
                _containerModuleSystem);
            chestRoomCreator.CreateChests(room, generationRoom);
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

        private void CreateDoors(Room room, DungeonGenerationRoom generationRoom)
        {
            var doors = new List<Door.Door>(generationRoom.Doors.Count);
            foreach (var generationDoor in generationRoom.Doors)
            {
                var data = new DoorData()
                {
                    Position = generationDoor.LocalPosition,
                    RequiredKey = generationDoor.RequiredKey,
                    IsClosed = generationDoor.IsRequiredKey
                };

                var tileModuleItem = _tilesController.CreateTileByGenerationID(DungeonTile.Door, generationDoor.LocalPosition);
                if (!tileModuleItem.HasValue)
                {
                    HLogger.LogError($"Cant create tile");
                    continue;
                }

                data.Reference = tileModuleItem.Value.ReferenceSelf;

                var door = new Door.Door(data, tileModuleItem.Value, room);
                
                doors.Add(door);
            }

            room.Doors = doors;
        }

        private Optional<Tile> CreateTile(
            GeneraitonTile generationTile,
            Vector2Int localPosition)
        {
            if (generationTile.Id == DungeonTile.Empty)
            {
                return Optional<Tile>.Fail();
            }
            
            var tileModuleItem = _tilesController.CreateTileByGenerationID(generationTile.Id, localPosition);
            if (!tileModuleItem.HasValue)
            {
                HLogger.LogError($"Cant create tile");
                return Optional<Tile>.Fail();
            }

            var data = new TileData();
            data.Reference = tileModuleItem.Value.ReferenceSelf;
            var tile = new Tile(data, tileModuleItem.Value);
            
            return Optional<Tile>.Success(tile);
        }
    }
}