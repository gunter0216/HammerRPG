using App.Common.Utilities.Utility.Runtime;
using App.Game.GameManagers.External.Fabric.Room.View;
using App.Game.GameManagers.External.Room;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Game.GameManagers.External.Fabric.Room
{
    public class RoomCreator
    {
        private readonly RoomViewCreator m_RoomViewCreator;

        public RoomCreator(RoomViewCreator roomViewCreator)
        {
            m_RoomViewCreator = roomViewCreator;
        }

        public Optional<GameRoom> Create(DungeonGenerationRoom generationRoom)
        {
            var view = m_RoomViewCreator.Create();
            if (!view.HasValue)
            {
                return Optional<GameRoom>.Fail();
            }
            
            var room = new GameRoom(view.Value, generationRoom);
            return Optional<GameRoom>.Success(room);
        }
    }
}