using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.DungeonCreator.Runtime.Rooms;
using App.Generation.DungeonCreator.Runtime;

namespace App.Game.DungeonCore.External.Services
{
    public class DungeonService
    {
        private readonly Dungeon _dungeon;
        private List<RoomService> _rooms;

        public Dungeon Dungeon => _dungeon;

        public IReadOnlyList<RoomService> Rooms => _rooms;

        public DungeonService(Dungeon dungeon)
        {
            _dungeon = dungeon;
        }

        public void Initialize()
        {
            _rooms = new List<RoomService>(16);
            
            foreach (var room in _rooms)
            {
                room.Initialize();
            }
            
            foreach (var room in _dungeon.Rooms)
            {
                var roomService = CreateRoom(room);
                if (!roomService.HasValue)
                {
                    HLogger.LogError("Cant create room");
                    return;
                }
                
                _rooms.Add(roomService.Value);
            }
        }

        public Vector2 GetSpawnPoint()
        {
            return _dungeon.StartRoom.GetCenter();
        }
        
        private Optional<RoomService> CreateRoom(Room room)
        {
            var roomService = new RoomService(room);
            
            return Optional<RoomService>.Success(roomService);
        }
    }
}