using App.Game.DungeonCreator.Runtime.Rooms;

namespace App.Game.DungeonCore.External.Services
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