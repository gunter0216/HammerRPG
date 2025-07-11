using System.Collections.Generic;
using App.Common.Algorithms.Runtime;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Triangulation.Cash
{
    public class TriangulationGenerationCash : IGenerationCash 
    {
        private readonly List<Triangle> m_Triangles;

        public TriangulationGenerationCash(List<Triangle> triangles)
        {
            m_Triangles = triangles;
        }

        public List<Triangle> Triangles => m_Triangles;
    }
}