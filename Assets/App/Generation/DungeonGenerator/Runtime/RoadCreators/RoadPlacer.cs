// using System.Collections.Generic;
// using App.Game.DungeonGenerator.Runtime.DungeonGenerators;
// using App.Game.DungeonGenerator.Runtime.Extensions;
// using App.Game.DungeonGenerator.Runtime.Utility;
// using App.Game.DungeonGenerator.Runtime.Utility.Loggers;
// using DungeonGenerator.Matrices;
//
// namespace App.Game.DungeonGenerator.Runtime.RoadCreators
// {
//     public class RoadPlacer
//     {
//         private readonly Position[] m_Offsets = new Position[]
//         {
//             new Position( 0, 1 ),
//             new Position( 0, -1 ),
//             new Position( -1, -1 ),
//             new Position( -1, 0 ),
//             new Position( -1, 1 ),
//             new Position( 1, -1 ),
//             new Position( 1, 0 ),
//             new Position( 1, 1 )
//         };
//     
//         private readonly Matrix m_CollidingMatrix;
//         private readonly Matrix m_InputMatrix;
//         private readonly int m_Thickness;
//
//         public RoadPlacer(Matrix collidingMatrix, Matrix inputMatrix, int thickness)
//         {
//             m_CollidingMatrix = collidingMatrix;
//             m_InputMatrix = inputMatrix;
//             m_Thickness = thickness;
//         }
//
//         public void PlaceRoad(List<Position> path, 
//             RoomSidePosition roomSidePositionFrom)
//         {
//             foreach (var position in path)
//             {
//                 m_InputMatrix.SetCell(position, Tile.Road);    
//             }
//         
//             foreach (var position in path)
//             {
//                 m_CollidingMatrix.SetCell(position, Tile.Wall);    
//             }
//
//             ReplaceRoomWallToRoad(roomSidePositionFrom);
//
//             PlaceWallsInInputMatrix(path);
//             PlaceWallsInCollidingMatrix(path);
//         }
//     
//         public void PlaceRoad(List<Position> path, 
//             RoomSidePosition roomSidePositionFrom, 
//             RoomSidePosition roomSidePositionTo)
//         {
//             foreach (var position in path)
//             {
//                 m_InputMatrix.SetCell(position, Tile.Road);    
//             }
//         
//             foreach (var position in path)
//             {
//                 m_CollidingMatrix.SetCell(position, Tile.Wall);    
//             }
//
//             ReplaceRoomWallToRoad(roomSidePositionFrom);
//             ReplaceRoomWallToRoad(roomSidePositionTo);
//
//             PlaceWallsInInputMatrix(path);
//             PlaceWallsInCollidingMatrix(path);
//         }
//
//         private void PlaceWallsInCollidingMatrix(List<Position> path)
//         {
//             for (int i = 0; i < path.Count - 1; ++i)
//             {
//                 int newCellValue;
//                 if (path[i].Row == path[i + 1].Row)
//                 {
//                     newCellValue = Tile.VerticalWall;
//                 }
//                 else if (path[i].Col == path[i + 1].Col)
//                 {
//                     newCellValue = Tile.HorizontalWall;
//                 }
//                 else
//                 {
//                     newCellValue = Tile.Wall;
//                     Logger.Log("Incorrect path");
//                 }
//             
//                 m_CollidingMatrix.SetCell(path[i], newCellValue);
//                 foreach (var offset in m_Offsets)
//                 {
//                     var newPosition = path[i] + offset;
//                     if (m_CollidingMatrix.IsCorrectPos(newPosition))
//                     {
//                         var cellValue = m_CollidingMatrix.GetCell(newPosition);
//                         if (cellValue == Tile.Empty)
//                         {
//                             m_CollidingMatrix.SetCell(newPosition, newCellValue);
//                         }
//                     }
//                 }
//             }
//         }
//
//         private void PlaceWallsInInputMatrix(List<Position> path)
//         {
//             for (int i = 0; i < path.Count; ++i)
//             {
//                 foreach (var offset in m_Offsets)
//                 {
//                     var newPosition = path[i] + offset;
//                     if (m_InputMatrix.IsCorrectPos(newPosition))
//                     {
//                         var cellValue = m_InputMatrix.GetCell(newPosition);
//                         if (cellValue == Tile.Wall)
//                         {
//                             m_InputMatrix.SetCell(newPosition, Tile.RoadWall);
//                         }
//                     }
//                 }
//             }
//         }
//
//         private void ReplaceRoomWallToRoad(RoomSidePosition roomSidePosition)
//         {
//             Position pos1 = roomSidePosition.Position;
//             Position pos2;
//             switch (roomSidePosition.Side)
//             {
//                 case (Side.Top):
//                     pos2 = roomSidePosition.Position.MoveBottom(m_Thickness);
//                     break;
//                 case (Side.Bottom):
//                     pos2 = roomSidePosition.Position.MoveTop(m_Thickness);
//                     break;
//                 case (Side.Left):
//                     pos2 = roomSidePosition.Position.MoveRight(m_Thickness);
//                     break;
//                 case (Side.Right):
//                     pos2 = roomSidePosition.Position.MoveLeft(m_Thickness);
//                     break;
//                 default:
//                     Logger.Log("Incorrect side");
//                     return;
//             }
//         
//             m_InputMatrix.SetLine(pos1, pos2, Tile.Road);
//         }
//     }
// }