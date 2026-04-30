using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Rooms;

namespace App.Game.Dungeon.DungeonCore.Runtime.Services
{
    public class DungeonService
    {
        private readonly DungeonCreator.Runtime.Dungeon _dungeon;
        private List<RoomService> _rooms;

        public DungeonCreator.Runtime.Dungeon Dungeon => _dungeon;

        public IReadOnlyList<RoomService> Rooms => _rooms;

        public DungeonService(DungeonCreator.Runtime.Dungeon dungeon)
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