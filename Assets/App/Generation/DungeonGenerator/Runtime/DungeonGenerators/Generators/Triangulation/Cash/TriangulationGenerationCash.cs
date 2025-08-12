using System.Collections.Generic;
using App.Generation.DungeonGenerator.Runtime.Rooms;
using Assets.App.Common.Algorithms.Runtime;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Triangulation.Cash
{
    public class TriangulationGenerationCash : IGenerationCash 
    {
        private readonly List<Triangle> m_Triangles;
        private readonly Dictionary<Vector2, DungeonGenerationRoom> m_PointToRoom;

        public TriangulationGenerationCash(
            List<Triangle> triangles, 
            Dictionary<Vector2, DungeonGenerationRoom> pointToRoom)
        {
            m_Triangles = triangles;
            m_PointToRoom = pointToRoom;
        }

        public List<Triangle> Triangles => m_Triangles;
        public Dictionary<Vector2, DungeonGenerationRoom> PointToRoom => m_PointToRoom;
    }
}