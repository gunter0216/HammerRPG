using System;
using System.Collections.Generic;
using App.Game.DungeonGenerator.Runtime.Rooms;
using DungeonGenerator.Matrices;

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

        private RoomSeparationStrategy m_SeparationStrategy = new RoomSeparationStrategy();
        private RoomCreateStrategy m_RoomCreateStrategy = new RoomCreateStrategy();

        public DungeonGenerator()
        {
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
            var matrix = new Matrix(dungeonConfig.Width, dungeonConfig.Height);
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
            var rooms = m_RoomCreateStrategy.CreateRooms(dungeon.Config);
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
	        var smallRooms = new HashSet<int>(dungeon.Data.RoomsData.Rooms.Count);
	        for(int i = 0; i < dungeon.Data.RoomsData.Rooms.Count; ++i)
	        {
		        var room = dungeon.Data.RoomsData.Rooms[i];
		        
		        if (room.Width < dungeon.Config.Rooms.WidthRoomThreshold || room.Height < dungeon.Config.Rooms.HeightRoomThreshold)
		        {
			        smallRooms.Add(i);
		        }
	        }

	        return smallRooms;
        }

        public void DiscardRooms(Dungeon dungeon, HashSet<int> discardRooms)
        {
	        var dungeonRooms = dungeon.Data.RoomsData.Rooms;
	        var newRooms = new List<DungeonRoomData>(dungeonRooms.Count);
	        for (int i = 0; i < dungeonRooms.Count; ++i)
	        {
		        if (!discardRooms.Contains(i))
		        {
			        newRooms.Add(dungeonRooms[i]);
		        }
	        }

	        dungeon.Data.RoomsData.Rooms = newRooms;
        }

        public HashSet<int> DiscardBorderingRooms(Dungeon dungeon)
        {
	        var minCorridorSize = dungeon.Config.Rooms.MinCorridorSize;
	        var roomsCount = dungeon.Data.RoomsData.Rooms.Count;

	        var borderingRooms = new HashSet<int>(dungeon.Data.RoomsData.Rooms.Count);
	        
	        for (int i = 0; i < roomsCount; ++i)
	        {
		        var room = dungeon.Data.RoomsData.Rooms[i];
		        
		        for (int j = i + 1; j < roomsCount; ++j)
		        {
			        var other = dungeon.Data.RoomsData.Rooms[j];
			        
			        var distance = room.GetCenter() - other.GetCenter();
			        var roomDistX = Math.Abs(distance.x);
			        var roomDistY = Math.Abs(distance.y);
			        
			        var minCorridorSizeSpaceX = room.Width / 2 + other.Width / 2 + minCorridorSize;
			        var minCorridorSizeSpaceY = room.Height / 2 + other.Height / 2 + minCorridorSize;
			        
			        var isCorridorFlat = roomDistX > roomDistY;
			        
			        if (isCorridorFlat && roomDistX < minCorridorSizeSpaceX ||
			            !isCorridorFlat && roomDistY < minCorridorSizeSpaceY)
			        {
				        borderingRooms.Add(i);
				        break;
			        }
		        }
	        }

	        return borderingRooms;
        }
    
        // private void PlaceRooms()
        // {
        //     while (m_PlacedRooms > 0)
        //     {
        //         var roomPos = GetRandomPos();
        //         var roomSize = GetRandomRoomSize();
        //         if (CanPlaceRoom(roomPos, roomSize))
        //         {
        //             // Logger.Log($"{roomPos} {roomSize}");
        //             PlaceRoom(roomPos, roomSize);
        //             m_PlacedRooms -= 1;
        //             m_Rooms.Add(new Room(roomPos, roomSize));
        //         }
        //     }
        // }
        //
        // private bool CanPlaceRoom(Position pos, Size size)
        // {
        //     for (int i = 0; i < size.Height; ++i)
        //     {
        //         for (int j = 0; j < size.Width; ++j)
        //         {
        //             if (!m_Matrix.IsCorrectPos(pos.Row + i, pos.Col + j))
        //             {
        //                 Logger.Log("Incorrect pos");
        //                 return false;
        //             }
        //
        //             if (m_Matrix.GetCell(pos.Row + i, pos.Col + j) != Tile.Wall)
        //             {
        //                 return false;
        //             }
        //         }
        //     }
        //
        //     return true;
        // }
        //
        // private void PlaceRoom(Position pos, Size size)
        // {
        //     for (int i = 0; i < size.Height; ++i)
        //     {
        //         for (int j = 0; j < size.Width; ++j)
        //         {
        //             if (i == 0 || i == size.Height - 1 ||
        //                 j == 0 || j == size.Width - 1)
        //             {
        //                 m_Matrix.SetCell(pos.Row + i, pos.Col + j, Tile.RoomWall);
        //             }
        //             else
        //             {
        //                 m_Matrix.SetCell(pos.Row + i, pos.Col + j, Tile.Empty);
        //             }
        //         }
        //     }
        // }
        //
        // private Position GetRandomPos()
        // {
        //     var col = m_Random.Next(0, DungeonConstants.Width + 1);
        //     var row = m_Random.Next(0, DungeonConstants.Height + 1);
        //     return new Position(row, col);
        // }
        //
        // private Size GetRandomRoomSize()
        // {
        //     var width = m_Random.Next(DungeonConstants.MinWidthRoom, DungeonConstants.MaxWidthRoom + 1);
        //     var height = m_Random.Next(DungeonConstants.MinHeightRoom, DungeonConstants.MaxHeightRoom + 1);
        //     return new Size(width, height);
        // }
        //
        // private void FillWalls()
        // {
        //     m_Matrix.Fill(Tile.Wall);
        // }
    }
}