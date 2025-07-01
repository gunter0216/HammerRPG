// using System.Collections.Generic;
// using App.Game.DungeonGenerator.Runtime.PathFinders;
// using App.Game.DungeonGenerator.Runtime.RoadCreators.CreatorCollidingMatrix;
// using App.Game.DungeonGenerator.Runtime.Rooms;
// using App.Game.DungeonGenerator.Runtime.Utility.Loggers;
// using DungeonGenerator.Matrices;
//
// namespace App.Game.DungeonGenerator.Runtime.RoadCreators
// {
//     /// <summary>
//     /// Соединяет по две комнаты дорогами
//     /// </summary>
//     public class RoadCreator1 : IRoadCreator
//     {
//         private const int m_Thickness = 1;
//     
//         // private readonly Random m_Random = new Random();
//         private readonly IPathFinder m_PathFinder;
//         private readonly RoadCreatorHelper m_RoadHelper;
//         private readonly ICreatorCollidingMatrix m_CreatorCollidingMatrix;
//
//         public RoadCreator1(IPathFinder pathFinder)
//         {
//             m_PathFinder = pathFinder;
//             m_RoadHelper = new RoadCreatorHelper(m_Thickness);
//             m_CreatorCollidingMatrix = new CreatorCollidingMatrix1();
//         }
//     
//         public void CreateRoads(List<Room> rooms, Matrix inputMatrix)
//         {
//             Matrix collidingMatrix = m_CreatorCollidingMatrix.CreateCollidingMatrix(inputMatrix, m_Thickness);
//             // collidingMatrix.Print();
//             RoadPlacer roadPlacer = new RoadPlacer(collidingMatrix, inputMatrix, m_Thickness);
//             LinkedList<Room> roomsWithoutRoad = new LinkedList<Room>(rooms);
//             int attempt = 1000;
//             while (--attempt >= 0)
//             {
//                 if (roomsWithoutRoad.Count <= 1)
//                 {
//                     return;
//                 }
//             
//                 var room1 = m_RoadHelper.GetRandomRoom(roomsWithoutRoad);
//                 var roomSidePosition1 = m_RoadHelper.GetRandomPosOnRandomRoomSideWithThickness(collidingMatrix, room1);
//             
//                 var room2 = m_RoadHelper.GetRandomRoom(roomsWithoutRoad);
//                 var roomSidePosition2 = m_RoadHelper.GetRandomPosOnRandomRoomSideWithThickness(collidingMatrix, room2);
//
//                 if (roomSidePosition1.IsEmpty() ||
//                     roomSidePosition2.IsEmpty())
//                 {
//                     Logger.Log("Cant create random pos on room side");
//                     attempt -= 10;
//                     continue;
//                 }
//             
//                 if (ReferenceEquals(room1, room2))
//                 {
//                     continue;
//                 }
//             
//                 var path = m_PathFinder.FindPath(collidingMatrix,
//                     roomSidePosition1.Position,
//                     roomSidePosition2.Position);
//
//                 if (!path.HasValue)
//                 {
//                     continue;
//                 }
//             
//                 roadPlacer.PlaceRoad(path.Value, roomSidePosition1, roomSidePosition2);
//
//                 roomsWithoutRoad.Remove(room1);
//                 roomsWithoutRoad.Remove(room2);
//             }
//
//             if (attempt <= 0)
//             {
//                 Logger.Log($"Cant generate road. Count rooms without roads {roomsWithoutRoad.Count}");    
//             }
//         }
//     }
// }