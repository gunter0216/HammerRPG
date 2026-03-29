using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Common.DataContainer.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.DungeonCreator.Runtime.Door;
using App.Game.DungeonCreator.Runtime.Tiles;
using App.Game.GameManagers.External.Room;
using App.Game.GameTiles.Runtime;
using App.Generation.DungeonCreator.Runtime.Chest;
using App.Generation.DungeonCreator.Runtime.Tiles;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Game.DungeonCreator.Runtime.Rooms
{
    public class RoomCreator
    {
        private readonly ITilesController _tilesController;
        private readonly IModuleItemsManager _moduleItemsManager;

        public RoomCreator(ITilesController tilesController, IModuleItemsManager moduleItemsManager)
        {
            _tilesController = tilesController;
            _moduleItemsManager = moduleItemsManager;
        }

        public Optional<Room> CreateRoom(DungeonGenerationRoom generationRoom)
        {
            var data = new RoomData(generationRoom)
            {
                Tiles = new List<TileData>()
            };
            var room = new Room(data);

            CreateWalls(room, generationRoom);
            CreateDoors(room, generationRoom);
            CreateChests(room, generationRoom);
            
            return Optional<Room>.Success(room);
        }

        private void CreateWalls(Room room, DungeonGenerationRoom generationRoom)
        {
            var matrix = generationRoom.Tiles;
            room.Tiles = new List<Tile>(64);
            foreach (var roomTile in generationRoom.Tiles)
            {
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

        private void CreateDoors(Room room, DungeonGenerationRoom generationRoom)
        {
            var doors = new List<Door>(generationRoom.Doors.Count);
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

                var door = new Door(data, tileModuleItem.Value, room);
                
                doors.Add(door);
            }

            room.Doors = doors;
        }

        private void CreateChests(Room room, DungeonGenerationRoom generationRoom)
        {
            var chests = new List<Chest>();
            room.Chests = chests;

            if (generationRoom.ContainsDoorKeys.Count <= 0)
            {
                return;
            }
            
            var localPosition = room.GetLocalCenter();
            var itemReferences = new List<DataReference>();
            var items = new List<IModuleItem>();
            var data = new ChestData()
            {
                Position = localPosition,
                State = ChestStateConstants.Closed,
                Items = itemReferences
            };

            var tileModuleItem = _tilesController.CreateTileByGenerationID(DungeonTile.Chest, localPosition);
            if (!tileModuleItem.HasValue)
            {
                HLogger.LogError($"Cant create tile");
                return;
            }

            data.Reference = tileModuleItem.Value.ReferenceSelf;

            var chest = new Chest(data, tileModuleItem.Value, room, items);

            chests.Add(chest);
            foreach (var keyData in generationRoom.ContainsDoorKeys)
            {
                var moduleItem = _moduleItemsManager.Create("IronKey");
                if (!moduleItem.HasValue)
                {
                    HLogger.LogError($"Cant create moduleItem");
                    return;
                }

                var keyModuleData = new KeyModuleData(keyData);
                if (!moduleItem.Value.AddDataModule(keyModuleData))
                {
                    HLogger.LogError("Cant add key data");
                    continue;
                }
                
                itemReferences.Add(moduleItem.Value.ReferenceSelf);
                items.Add(moduleItem.Value);
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