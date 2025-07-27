using System.Collections.Generic;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding.Cash
{
    public class BorderingRoomsGenerationCash : IGenerationCash
    {
        private readonly HashSet<int> m_Rooms;

        public HashSet<int> Rooms => m_Rooms;

        public BorderingRoomsGenerationCash(HashSet<int> rooms)
        {
            m_Rooms = rooms;
        }
    }
}