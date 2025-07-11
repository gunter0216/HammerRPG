using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Common.Utility.Runtime;
using App.Generation.DelaunayTriangulation.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Triangulation.Cash;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Triangulation
{
    public class TriangulationDungeonGenerator : IDungeonGenerator
    {
        private readonly DelaunayTriangulation.Runtime.DelaunayTriangulation m_Triangulation;

        public TriangulationDungeonGenerator()
        {
            m_Triangulation = new DelaunayTriangulation.Runtime.DelaunayTriangulation();
        }

        public Optional<DungeonGeneration> Process(DungeonGeneration generation)
        {
            var dungeon = generation.Dungeon;
            var triangles = Triangulate(dungeon);
            generation.AddCash(new TriangulationGenerationCash(triangles));
            
            return Optional<DungeonGeneration>.Success(generation);
        }

        private List<Triangle> Triangulate(Dungeon dungeon)
        {
            var points = new List<Vector2>();
            foreach (var roomData in dungeon.Data.RoomsData.Rooms)
            {
                var center = roomData.GetCenter();
                var point = new Vector2(center.X, center.Y);
                points.Add(point);
            }
        
            var triangles = m_Triangulation.Triangulate(points);
            
            return triangles;
        }

        public string GetName()
        {
            return "Triangulation";
        }
    }
}