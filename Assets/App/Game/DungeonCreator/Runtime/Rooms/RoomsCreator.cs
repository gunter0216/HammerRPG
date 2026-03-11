using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.GameManagers.External.Config.Service;
using App.Game.GameManagers.External.Room;
using App.Game.GameTiles.Runtime;
using App.Generation.DungeonCreator.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using App.Game.DungeonCreator.Runtime.Tiles;
using App.Generation.DungeonCreator.Runtime.Tiles;
using TileConstants = App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.TileConstants;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Game.GameManagers.External.Fabric.Room
{
    public class RoomsCreator
    {
        private readonly ITilesController m_TilesController;

        private GenerationConfigController m_ConfigController;

        public RoomsCreator(ITilesController tilesController)
        {
            m_TilesController = tilesController;
        }

        public void CreateRooms(DungeonGeneration generation, Dungeon dungeon)
        {
            var dataGenerationRooms = generation.DungeonGenerationResult.GenerationData.GenerationRooms;
            var startRoom = dataGenerationRooms.StartGenerationRoom;
            var endRoom = dataGenerationRooms.EndGenerationRoom;
            var generationRooms = dataGenerationRooms.Rooms;

            var rooms = new List<DungeonCreator.Runtime.Rooms.Room>(generationRooms.Count);
            foreach (var generationRoom in generationRooms)
            {
                var room = CreateRoom(generationRoom);
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

        private Optional<DungeonCreator.Runtime.Rooms.Room> CreateRoom(DungeonGenerationRoom generationRoom)
        {
            var data = new RoomData(generationRoom)
            {
                Tiles = new List<TileData>()
            };
            var room = new DungeonCreator.Runtime.Rooms.Room(data);

            CreateTiles(room, generationRoom);
            
            return Optional<DungeonCreator.Runtime.Rooms.Room>.Success(room);
        }

        private void CreateTiles(DungeonCreator.Runtime.Rooms.Room room, DungeonGenerationRoom generationRoom)
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

        private Optional<Tile> CreateTile(
            GeneraitonTile generationTile,
            Vector2Int localPosition)
        {
            if (generationTile.Id == TileConstants.Empty)
            {
                return Optional<Tile>.Fail();
            }
            
            var tileModuleItem = m_TilesController.CreateTileByGenerationID(generationTile.Id, localPosition);
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