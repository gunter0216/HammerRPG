using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.DungeonCreator.Runtime.Door;
using App.Game.DungeonCreator.Runtime.Tiles;
using App.Game.GameManagers.External.Room;
using App.Game.GameTiles.Runtime;
using App.Generation.DungeonCreator.Runtime.Tiles;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using TileConstants = App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.TileConstants;

namespace App.Game.DungeonCreator.Runtime.Rooms
{
    public class RoomCreator
    {
        private readonly ITilesController _tilesController;

        public RoomCreator(ITilesController tilesController)
        {
            _tilesController = tilesController;
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
            
            return Optional<Room>.Success(room);
        }

        private void CreateWalls(Room room, DungeonGenerationRoom generationRoom)
        {
            var matrix = generationRoom.Matrix;
            room.Tiles = new List<Tile>(64);
            for (int i = 0; i < matrix.Height; ++i)
            {
                for (int j = 0; j < matrix.Width; ++j)
                {
                    var generationTile = matrix[i, j];
                    var position = new Vector2Int(j, i);
                    var tile = CreateTile(generationTile, new Vector2Int(j, i));
                    if (!tile.HasValue)
                    {
                        continue;
                    }

                    tile.Value.Data.Position = position;
                    room.Tiles.Add(tile.Value);
                    room.Data.Tiles.Add(tile.Value.Data);
                }
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

                var tileModuleItem = _tilesController.CreateTileByGenerationID("Door", generationDoor.LocalPosition);
                if (!tileModuleItem.HasValue)
                {
                    HLogger.LogError($"Cant create tile");
                    continue;
                }

                data.Reference = tileModuleItem.Value.ReferenceSelf;

                var door = new Door(data, tileModuleItem.Value);
                
                doors.Add(door);
            }

            room.Doors = doors;
        }

        private Optional<Tile> CreateTile(
            GeneraitonTile generationTile,
            Vector2Int localPosition)
        {
            if (generationTile.Id == TileConstants.Empty)
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