using System;
using System.Collections.Generic;
using System.Linq;
using App.Game.DelaunayTriangulation.Runtime;
using App.Game.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Game.DungeonGenerator.Runtime.DungeonGenerators.Generation.BorderingRoomsDiscarding;
using App.Game.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsCreator;
using App.Game.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsDiscarding;
using App.Game.DungeonGenerator.Runtime.DungeonGenerators.Generation.RoomsSeparator;
using App.Game.DungeonGenerator.Runtime.Rooms;
using App.Game.KruskalAlgorithm.Runtime;
using UnityEngine;
using Edge = App.Game.DelaunayTriangulation.Runtime.Edge;

namespace App.Game.DungeonGenerator.Runtime.DungeonGenerators
{
    public class DungeonGenerator
    {
        // private readonly Matrix m_Matrix = new Matrix(DungeonConstants.Width, DungeonConstants.Height);
        // private readonly Random m_Random = new Random();
        // private readonly IPathFinder m_PathFinder;
        // private readonly IRoadCreator m_RoadCreator;
        // private readonly ICellFinder m_CellFinder;
        //
        // private int m_PlacedRooms = DungeonConstants.CountRooms;
        //
        // private List<Room> m_Rooms;

        private RoomsSeparator m_SeparationStrategy;
        private RoomsCreator m_RoomsCreator;
        private SmallRoomsDiscarder m_SmallRoomsDiscarder;
        private BorderingRoomsDiscarder m_BorderingRoomsDiscarder;
        private ITriangulation m_Triangulation;
        private KruskalAlgorithm.Runtime.KruskalAlgorithm m_KruskalAlgorithm;

        public DungeonGenerator()
        {
            var roomCreator = new RoomCreator();
            m_SeparationStrategy = new RoomsSeparator();
            m_RoomsCreator = new RoomsCreator(roomCreator);
            m_SmallRoomsDiscarder = new SmallRoomsDiscarder();
            m_BorderingRoomsDiscarder = new BorderingRoomsDiscarder();
            m_Triangulation = new DelaunayTriangulation.Runtime.DelaunayTriangulation();
            m_KruskalAlgorithm = new KruskalAlgorithm.Runtime.KruskalAlgorithm();
            // m_Rooms = new List<Room>(DungeonConstants.CountRooms);
            // m_PathFinder = new PathFinder(
            //     Tile.Wall, 
            //     Tile.Empty, 
            //     Tile.HorizontalWall, 
            //     Tile.VerticalWall);
            // m_CellFinder = new CellFinder(
            //     Tile.Wall, 
            //     Tile.Empty, 
            //     Tile.HorizontalWall, 
            //     Tile.VerticalWall);
            // m_RoadCreator = new RoadCreator2(m_PathFinder, m_CellFinder);
        }

        public Dungeon Generate(DungeonConfig dungeonConfig)
        {
            var matrix = new Matrix.Matrix(dungeonConfig.Width, dungeonConfig.Height);
            var dungeonData = new DungeonData();
            dungeonData.RoomsData = new DungeonRoomsData();
            dungeonData.Matrix = matrix;
            var dungeon = new Dungeon(dungeonConfig, dungeonData);

            // FillWalls();
            // PlaceRooms();
            // m_RoadCreator.CreateRoads(m_Rooms, m_Matrix);
            // MatrixPrinter.Print(m_Matrix);
            // Matrix<int> scaledMatrix = m_Matrix.Scale(2);
            // // MatrixPrinter.Print(scaledMatrix);

            return dungeon;
        }

        public void GenerateRooms(Dungeon dungeon)
        {
            var rooms = m_RoomsCreator.CreateRooms(dungeon.Config);
            dungeon.Data.RoomsData.Rooms = rooms;
        }

        public bool SeparateRooms(Dungeon dungeon)
        {
            return m_SeparationStrategy.Separate(dungeon);
            // m_SeparationStrategy.SeparateRectangles(dungeon);
            // m_SeparationStrategy.SeparateRectanglesWithGrid(dungeon);
            return true;
        }

        public HashSet<int> GetSmallRooms(Dungeon dungeon)
        {
	        return m_SmallRoomsDiscarder.GetSmallRooms(dungeon);
        }

        public void DiscardRooms(Dungeon dungeon, HashSet<int> discardRooms)
        {
	        m_SmallRoomsDiscarder.DiscardRooms(dungeon, discardRooms);
        }

        public HashSet<int> GetBorderingRooms(Dungeon dungeon)
        {
            return m_BorderingRoomsDiscarder.GetBorderingRooms(dungeon);
        }

        public List<Triangle> Triangulate(Dungeon dungeon)
        {
            var points = new List<Point>();
            foreach (var roomData in dungeon.Data.RoomsData.Rooms)
            {
                var center = roomData.GetCenter();
                var point = new Point(center.x, center.y);
                points.Add(point);
            }

            foreach (var point in points)
            {
                Debug.LogError($"{point}");
            }

            var triangles = m_Triangulation.Triangulate(points);
            foreach (var triangle in triangles)
            {
                Debug.LogError($"{triangle}");
            }
            
            return triangles;
        }

        public List<Edge> GetEdges()
        {
            return m_Triangulation.GetEdges();
        }

        public (KruskalResult result, Dictionary<int, Point> indexToPoint) FindMinimumSpanningTree(List<Triangle> triangles)
        {
            return m_KruskalAlgorithm.FindMinimumSpanningTree(triangles);
        }
    }
}