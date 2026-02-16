using System.Collections.Generic;
using App.Generation.DungeonCreator.Runtime;

namespace App.Game.DungeonCore.External.Services
{
    public class DungeonService
    {
        private readonly Dungeon _dungeon;
        private readonly IReadOnlyList<RoomService> _rooms;

        public Dungeon Dungeon => _dungeon;

        public IReadOnlyList<RoomService> Rooms => _rooms;

        public DungeonService(Dungeon dungeon, IReadOnlyList<RoomService> rooms)
        {
            _dungeon = dungeon;
            _rooms = rooms;
        }

        public void Initialize()
        {
            foreach (var room in _rooms)
            {
                room.Initialize();
            }
        }
    }
}