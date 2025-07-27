using System.Collections.Generic;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common
{
    public class DungeonTreeResult
    {
        private readonly Dictionary<int, DungeonGenerationRoom> m_IndexToRoom;
        private readonly Dictionary<int, int> m_UIDToIndex;
        private readonly List<(int, int)> m_Edges;
        private readonly int m_Vertices;

        public DungeonTreeResult(Dictionary<int, DungeonGenerationRoom> indexToRoom,
            Dictionary<int, int> uidToIndex,
            List<(int, int)> edges, 
            int vertices)
        {
            m_IndexToRoom = indexToRoom;
            m_UIDToIndex = uidToIndex;
            m_Edges = edges;
            m_Vertices = vertices;
        }

        public Dictionary<int, DungeonGenerationRoom> IndexToRoom => m_IndexToRoom;

        public Dictionary<int, int> UIDToIndex => m_UIDToIndex;

        public List<(int, int)> Edges => m_Edges;

        public int Vertices => m_Vertices;
    }
}