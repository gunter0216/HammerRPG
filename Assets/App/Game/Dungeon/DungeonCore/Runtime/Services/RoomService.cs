using App.Game.Dungeon.DungeonCreator.Runtime.Rooms;

namespace App.Game.Dungeon.DungeonCore.Runtime.Services
{
    public class RoomService
    {
        private readonly Room _room;

        public Room Room => _room;

        public RoomService(Room room)
        {
            _room = room;
        }

        public void Initialize()
        {
            
        }
    }
}