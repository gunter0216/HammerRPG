using App.Game.DungeonGenerator.Runtime.Rooms;
using UnityEngine;

namespace App.Game.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsCreator
{
    public class RoomCreator
    {
        private static int m_Index;
        
        public DungeonRoomData Create(Vector2Int position, Vector2Int size)
        {
            var uid = m_Index++;
            var room = new DungeonRoomData(uid, position, size);
            return room;
        }
    }
}