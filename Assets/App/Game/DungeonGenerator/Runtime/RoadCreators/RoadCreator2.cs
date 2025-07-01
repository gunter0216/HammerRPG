// using System.Collections.Generic;
// using System.Linq;
// using App.Game.DungeonGenerator.Runtime.CellFinders;
// using App.Game.DungeonGenerator.Runtime.DungeonGenerators;
// using App.Game.DungeonGenerator.Runtime.Extensions;
// using App.Game.DungeonGenerator.Runtime.PathFinders;
// using App.Game.DungeonGenerator.Runtime.RoadCreators.CreatorCollidingMatrix;
// using App.Game.DungeonGenerator.Runtime.Rooms;
// using App.Game.DungeonGenerator.Runtime.Utility;
// using App.Game.DungeonGenerator.Runtime.Utility.Loggers;
// using DungeonGenerator.Matrices;
//
// namespace App.Game.DungeonGenerator.Runtime.RoadCreators
// {
//     /// <summary>
//     /// 
//     /// </summary>
//     public class RoadCreator2 : IRoadCreator
//     {
//         private const int m_Thickness = 1;
//
//         private readonly IPathFinder m_PathFinder;
//         private readonly RoadCreatorHelper m_RoadHelper;
//         private readonly ICreatorCollidingMatrix m_CreatorCollidingMatrix;
//         private readonly ICellFinder m_CellFinder;
//
//         private RoadPlacer m_RoadPlacer;
//         private LinkedList<Room> m_RoomsWithoutRoad;
//         private Matrix m_CollidingMatrix;
//         private Matrix m_InputMatrix;
//
//         public RoadCreator2(IPathFinder pathFinder, ICellFinder cellFinder)
//         {
//             m_CellFinder = cellFinder;
//             m_PathFinder = pathFinder;
//             m_RoadHelper = new RoadCreatorHelper(m_Thickness);
//             m_CreatorCollidingMatrix = new CreatorCollidingMatrix2();
//         }
//     
//         public void CreateRoads(List<Room> rooms, Matrix inputMatrix)
//         {
//             m_InputMatrix = inputMatrix;
//             m_CollidingMatrix = m_CreatorCollidingMatrix.CreateCollidingMatrix(inputMatrix, m_Thickness);
//             m_RoadPlacer = new RoadPlacer(m_CollidingMatrix, inputMatrix, m_Thickness);
//             m_RoomsWithoutRoad = new LinkedList<Room>(rooms);
//
//             foreach (var room in m_RoomsWithoutRoad)
//             {
//                 Logger.Log($"{room.Position}");
//             }
//         
//             CreateRoad(Side.BottomLeft, Side.TopRight);
//             CreateRoad(Side.TopLeft, Side.BottomRight);
//             CreateRoad(Side.Top, Side.Bottom);
//             CreateRoad(Side.Left, Side.Right);
//
//             int attempt = 1000;
//             while (--attempt >= 0)
//             {
//                 if (m_RoomsWithoutRoad.Count < 1)
//                 {
//                     return;
//                 }
//
//                 var room = m_RoadHelper.GetRandomRoom(m_RoomsWithoutRoad);
//                 if (!TryCreateRoadFromRoomToClosestRoad(room))
//                 {
//                     attempt -= 20;
//                 }
//              
//                 //     var room1 = m_RoadHelper.GetRandomRoom(m_RoomsWithoutRoad);
//                 //     var roomSidePosition1 = m_RoadHelper.GetRandomPosOnRandomRoomSideWithThickness(m_CollidingMatrix, room1);
//                 //     
//                 //     var room2 = m_RoadHelper.GetRandomRoom(m_RoomsWithoutRoad);
//                 //     var roomSidePosition2 = m_RoadHelper.GetRandomPosOnRandomRoomSideWithThickness(m_CollidingMatrix, room2);
//                 //
//                 //     if (roomSidePosition1.IsEmpty() ||
//                 //         roomSidePosition2.IsEmpty())
//                 //     {
//                 //         Logger.Log("Cant create random pos on room side");
//                 //         attempt -= 10;
//                 //         continue;
//                 //     }
//                 //     
//                 //     if (ReferenceEquals(room1, room2))
//                 //     {
//                 //         continue;
//                 //     }
//                 //     
//                 //     var path = m_PathFinder.FindPath(m_CollidingMatrix,
//                 //         roomSidePosition1.Position,
//                 //         roomSidePosition2.Position);
//                 //
//                 //     if (!path.HasValue)
//                 //     {
//                 //         continue;
//                 //     }
//                 //     
//                 //     m_RoadPlacer.PlaceRoad(path.Value, roomSidePosition1, roomSidePosition2);
//                 //
//                 //     m_RoomsWithoutRoad.Remove(room1);
//                 //     m_RoomsWithoutRoad.Remove(room2);
//             }
//         
//         
//             if (attempt <= 0)
//             {
//                 Logger.Log($"Cant generate road. Count rooms without roads {m_RoomsWithoutRoad.Count}");    
//             }
//         }
//
//         private bool TryCreateRoadFromRoomToClosestRoad(Room room)
//         {
//             var roomSidePosition = m_RoadHelper.GetRandomPosOnRandomRoomSideWithThickness(m_CollidingMatrix, room);
//             if (roomSidePosition.IsEmpty())
//             {
//                 Logger.Log("Cant create random pos on room side");
//                 return false;
//             }
//         
//             // MatrixPrinter.Print(m_CollidingMatrix);
//         
//             var path = m_CellFinder.FindPath(m_CollidingMatrix,
//                 roomSidePosition.Position, 
//                 Tile.VerticalWall,
//                 Tile.HorizontalWall);
//         
//             if (!path.HasValue)
//             {
//                 Logger.Log("false");
//                 return false;
//             }
//
//             foreach (var position in path.Value)
//             {
//                 Logger.Log(position);
//             }
//         
//             m_RoadPlacer.PlaceRoad(path.Value, roomSidePosition);
//         
//             m_RoomsWithoutRoad.Remove(room);
//             return true;
//         }
//     
//         private void CreateRoad(Side from, Side to)
//         {
//             var attempt = 100;
//             var sortedRooms1 = GetSortedRooms(m_RoomsWithoutRoad, m_InputMatrix, from);
//             var sortedRooms2 = GetSortedRooms(m_RoomsWithoutRoad, m_InputMatrix, to);
//             var room1 = sortedRooms1.First();
//             var room2 = sortedRooms2.First();
//
//             if (ReferenceEquals(room1, room2))
//             {
//                 return;
//             }
//         
//             while (--attempt >= 0)
//             {
//                 if (!TryCreateRoad(room1, room2))
//                 {
//                     attempt -= 1;
//                     m_RoomsWithoutRoad.Remove(room1);
//                     m_RoomsWithoutRoad.Remove(room2);
//                     continue;
//                 }
//             
//                 break;
//             }
//         }
//     
//         private bool TryCreateRoad(Room room1, Room room2)
//         {
//             var roomSidePosition1 = m_RoadHelper.GetRandomPosOnRandomRoomSideWithThickness(m_CollidingMatrix, room1);
//             var roomSidePosition2 = m_RoadHelper.GetRandomPosOnRandomRoomSideWithThickness(m_CollidingMatrix, room2);
//
//             if (roomSidePosition1.IsEmpty() ||
//                 roomSidePosition2.IsEmpty())
//             {
//                 Logger.Log("Cant create random pos on room side");
//                 return false;
//             }
//             
//             var path = m_PathFinder.FindPath(m_CollidingMatrix,
//                 roomSidePosition1.Position,
//                 roomSidePosition2.Position);
//
//             if (!path.HasValue)
//             {
//                 return false;
//             }
//             
//             m_RoadPlacer.PlaceRoad(path.Value, roomSidePosition1, roomSidePosition2);
//
//             m_RoomsWithoutRoad.Remove(room1);
//             m_RoomsWithoutRoad.Remove(room2);
//             
//             return true;
//         }
//     
//         private List<Room> GetSortedRooms(ICollection<Room> rooms, Matrix matrix, Side from)
//         {
//             List<LengthNode<Room>> sortedRooms = new List<LengthNode<Room>>(rooms.Count);
//
//             Position sidePosition = matrix.GetPositionFromSide(from);
//             foreach (var room in rooms)
//             {
//                 int distance = sidePosition.SqrMagnitude(room.GetCenter());
//                 LengthNode<Room> node = new LengthNode<Room>(room, distance);
//                 sortedRooms.Add(node);
//             }
//         
//             sortedRooms.Sort();
//         
//             return sortedRooms.Select(x => x.Value).ToList();
//         }
//     }
// }