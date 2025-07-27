using System.Collections.Generic;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel
{
    public class DungeonGenerationResult
    {
        private readonly DungeonGenerationData m_GenerationData;
        
        public DungeonGenerationData GenerationData => m_GenerationData;

        public DungeonGenerationResult(DungeonGenerationData generationData)
        {
            m_GenerationData = generationData;
        }
        
        internal void DiscardRooms(HashSet<int> discardRooms)
        {
            var dungeonRooms = m_GenerationData.GenerationRooms.Rooms;
            var newRooms = new List<DungeonGenerationRoom>(dungeonRooms.Count);
            for (int i = 0; i < dungeonRooms.Count; ++i)
            {
                var room = dungeonRooms[i];
                if (!discardRooms.Contains(room.UID))
                {
                    newRooms.Add(room);
                }
            }

            m_GenerationData.GenerationRooms.Rooms = newRooms;
        }
    }
}