using System.Collections.Generic;
using App.Common.Algorithms.Runtime;
using App.Common.Utility.Runtime;
using App.Generation.DelaunayTriangulation.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Triangulation.Cash;
using App.Generation.DungeonGenerator.Runtime.Rooms;

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
            var dungeon = generation.DungeonGenerationResult;
            var cash = Triangulate(dungeon);
            generation.AddCash(cash);
            
            return Optional<DungeonGeneration>.Success(generation);
        }

        private TriangulationGenerationCash Triangulate(DungeonGenerationResult dungeonGenerationResult)
        {
            var points = new List<Vector2>();
            var pointToIndex = new Dictionary<Vector2, DungeonGenerationRoom>();
            foreach (var roomData in dungeonGenerationResult.GenerationData.GenerationRooms.Rooms)
            {
                var center = roomData.GetCenter();
                var point = new Vector2(center.X, center.Y);
                points.Add(point);
                pointToIndex.Add(center, roomData);
            }
        
            var triangles = m_Triangulation.Triangulate(points);
            
            return new TriangulationGenerationCash(triangles, pointToIndex);
        }

        public string GetName()
        {
            return "Triangulation";
        }
    }
}