using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.DungeonCreator.Runtime.Tiles;
using App.Game.GameManagers.External.Config.Service;
using App.Game.GameManagers.External.Room;
using App.Game.GameTiles.Runtime;
using App.Generation.DungeonCreator.Runtime;
using App.Generation.DungeonCreator.Runtime.Tiles;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using TileConstants = App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.TileConstants;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Game.DungeonCreator.Runtime.Rooms
{
    public class RoomsCreator
    {
        private readonly ITilesController _tilesController;
        private readonly RoomCreator _roomCreator;

        public RoomsCreator(ITilesController tilesController)
        {
            _tilesController = tilesController;
            _roomCreator = new RoomCreator(_tilesController);
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