using System.Collections.Generic;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SmallRoomsDiscarding.Cash
{
    public class SmallRoomsGenerationCash : IGenerationCash
    {
        private readonly HashSet<int> m_SmallRooms;

        public HashSet<int> SmallRooms => m_SmallRooms;

        public SmallRoomsGenerationCash(HashSet<int> smallRooms)
        {
            m_SmallRooms = smallRooms;
        }
    }
}