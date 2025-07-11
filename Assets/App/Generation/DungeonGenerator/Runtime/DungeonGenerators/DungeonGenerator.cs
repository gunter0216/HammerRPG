using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.Utility.Runtime;
using App.Generation.DelaunayTriangulation.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Generation;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsCreator;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsSeparator;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SmallRoomsDiscarding;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SpanningTree;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Triangulation;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators
{
    public class DungeonGenerator
    {
        private readonly ILogger m_Logger;
        
        private SeparateRoomsDungeonGenerator m_SeparationStrategy;
        private CreateRoomsDungeonGenerator m_CreateRoomsDungeonGenerator;
        private DiscardSmallRoomsDungeonGenerator m_DiscardSmallRoomsDungeonGenerator;
        private DiscardBorderingRoomsDungeonGenerator m_DiscardBorderingRoomsDungeonGenerator;
        private ITriangulation m_Triangulation;
        private KruskalAlgorithm.Runtime.KruskalAlgorithm m_KruskalAlgorithm;

        private readonly List<IDungeonGenerator> m_Generators;
        
        private int m_CurrentGeneratorIndex;
        private DungeonGeneration m_Generation;

        public DungeonGenerator(ILogger logger)
        {
            m_Logger = logger;
            var generators = new List<IDungeonGenerator>();
            
            var roomCreator = new RoomCreator();

            generators.Add(new CreateRoomsDungeonGenerator(roomCreator));
            generators.Add(new SeparateRoomsDungeonGenerator());
            generators.Add(new SelectSmallRoomsDungeonGenerator());
            generators.Add(new DiscardSmallRoomsDungeonGenerator());
            generators.Add(new SelectBorderingRoomsDungeonGenerator());
            generators.Add(new DiscardBorderingRoomsDungeonGenerator());
            generators.Add(new TriangulationDungeonGenerator());
            generators.Add(new SpanningTreeDungeonGenerator());
            
            m_Generators = generators;
        }

        public void StartGeneration(DungeonGenerationConfig generationConfig)
        {
            // var matrix = new Matrix.Matrix(dungeonConfig.Width, dungeonConfig.Height);
            var data = new DungeonData
            {
                RoomsData = new DungeonRoomsData(),
                // Matrix = matrix
            };
            var config = new DungeonConfig();
            var dungeon = new Dungeon(config, data);
            
            m_Generation = new DungeonGeneration(dungeon, generationConfig);
            m_CurrentGeneratorIndex = 0;
        }

        public bool NextIteration()
        {
            if (IsComplete())
            {
                m_Logger.LogError($"Generation complete!");
                return false;
            }

            var generator = m_Generators[m_CurrentGeneratorIndex];
            
            m_Logger.LogError($"Next iteration generation: {generator.GetName()}");
            
            var generation = generator.Process(m_Generation);
            if (!generation.HasValue)
            {
                m_Logger.LogError($"Ops. Something wrong. Cant generate next iteration.");
                return false;
            }

            m_CurrentGeneratorIndex += 1;
            m_Generation = generation.Value;
            
            return true;
        }

        public bool IsStart()
        {
            return m_Generation != null;
        }

        public bool IsComplete()
        {
            return m_CurrentGeneratorIndex >= m_Generators.Count;
        }

        public Optional<DungeonGeneration> GetGeneration()
        {
            if (m_Generation == null)
            {
                return Optional<DungeonGeneration>.Fail();
            }
            
            return Optional<DungeonGeneration>.Success(m_Generation);
        }

        // public void GenerateRooms(Dungeon dungeon)
        // {
        //     // var rooms = m_CreateRoomsDungeonGenerator.CreateRooms(dungeon.Config);
        //     // dungeon.Data.RoomsData.Rooms = rooms;
        // }
        //
        // public bool SeparateRooms(Dungeon dungeon)
        // {
        //     return m_SeparationStrategy.Separate(dungeon);
        //     // m_SeparationStrategy.SeparateRectangles(dungeon);
        //     // m_SeparationStrategy.SeparateRectanglesWithGrid(dungeon);
        //     return true;
        // }
        //
        // public HashSet<int> GetSmallRooms(Dungeon dungeon)
        // {
	       //  return m_SmallRoomsDiscarder.GetSmallRooms(dungeon);
        // }
        //
        // public void DiscardRooms(Dungeon dungeon, HashSet<int> discardRooms)
        // {
	       //  m_SmallRoomsDiscarder.DiscardRooms(dungeon, discardRooms);
        // }
        //
        // public HashSet<int> GetBorderingRooms(Dungeon dungeon)
        // {
        //     return m_BorderingRoomsDiscarder.GetBorderingRooms(dungeon);
        // }
        //
        // public List<Triangle> Triangulate(Dungeon dungeon)
        // {
        //     var points = new List<Point>();
        //     foreach (var roomData in dungeon.Data.RoomsData.Rooms)
        //     {
        //         var center = roomData.GetCenter();
        //         var point = new Point(center.x, center.y);
        //         points.Add(point);
        //     }
        //
        //     foreach (var point in points)
        //     {
        //         Debug.LogError($"{point}");
        //     }
        //
        //     var triangles = m_Triangulation.Triangulate(points);
        //     foreach (var triangle in triangles)
        //     {
        //         Debug.LogError($"{triangle}");
        //     }
        //     
        //     return triangles;
        // }
        //
        // public List<Edge> GetEdges()
        // {
        //     return m_Triangulation.GetEdges();
        // }
        //
        // public (KruskalResult result, Dictionary<int, Point> indexToPoint) FindMinimumSpanningTree(List<Triangle> triangles)
        // {
        //     return m_KruskalAlgorithm.FindMinimumSpanningTree(triangles);
        // }
    }
}