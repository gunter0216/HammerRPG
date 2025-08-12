using App.Generation.DungeonGenerator.Runtime.Rooms;
using Assets.App.Common.Algorithms.Runtime;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common
{
    public class RoomCreator
    {
        private static int m_Index;
        
        public DungeonGenerationRoom Create(Vector2Int position, Vector2Int size)
        {
            var uid = m_Index++;
            var room = new DungeonGenerationRoom(uid, position, size);
            return room;
        }
    }
}