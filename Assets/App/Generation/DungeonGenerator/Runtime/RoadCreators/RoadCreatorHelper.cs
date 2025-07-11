// using System;
// using System.Collections.Generic;
// using System.Linq;
// using App.Game.DungeonGenerator.Runtime.Rooms;
// using App.Game.DungeonGenerator.Runtime.Utility;
// using DungeonGenerator.Matrices;
//
// namespace App.Game.DungeonGenerator.Runtime.RoadCreators
// {
//     public class RoadCreatorHelper
//     {
//         private readonly Random m_Random = new Random();
//         private readonly int m_Thickness;
//
//         public RoadCreatorHelper(int thickness)
//         {
//             m_Thickness = thickness;
//         }
//     
//         public Room GetRandomRoom(IEnumerable<Room> rooms)
//         {
//             var enumerable = rooms.ToList();
//             var roomIndex = m_Random.Next(0, enumerable.Count);
//             return enumerable[roomIndex];
//         }
//     
//         public Room GetRandomRoom(List<Room> rooms)
//         {
//             var roomIndex = m_Random.Next(0, rooms.Count);
//             return rooms[roomIndex];
//         }
//
//         public RoomSidePosition GetRandomPosOnRandomRoomSideWithThickness(Matrix matrix, Room room)
//         {
//             var side = GetRandomSide();
//             var position = GetRandomPosOnRoomSideWithThickness(matrix, room, side);
//             return new RoomSidePosition(position, side);
//         }
//
//         public Position GetRandomPosOnRoomSideWithThickness(Matrix matrix, Room room, Side side)
//         {
//             int attempt = 100;
//             while (--attempt >= 0)
//             {
//                 Position position = Position.Empty;
//                 if (side == Side.Top)
//                 {
//                     int row = room.Position.Row - m_Thickness;
//                     int col = m_Random.Next(1, room.Size.Width - 1) + room.Position.Col;
//                     position = new Position(row, col);
//                 }
//                 else if (side == Side.Bottom)
//                 {
//                     int row = room.Position.Row + room.Size.Height - 1 + m_Thickness;
//                     int col = m_Random.Next(1, room.Size.Width - 1) + room.Position.Col;
//                     position = new Position(row, col);
//                 }
//                 else if (side == Side.Left)
//                 {
//                     int row = m_Random.Next(1, room.Size.Height - 1) + room.Position.Row;
//                     int col = room.Position.Col - m_Thickness;
//                     position = new Position(row, col);
//                 }
//                 else if (side == Side.Right)
//                 {
//                     int row = m_Random.Next(1, room.Size.Height - 1) + room.Position.Row;
//                     int col = room.Position.Col + room.Size.Width - 1 + m_Thickness;
//                     position = new Position(row, col);
//                 }
//
//                 if (matrix.IsCorrectPos(position))
//                 {
//                     return position;
//                 }
//             }
//
//             return Position.Empty;
//         }
//
//         public RoomSidePosition GetRandomPosOnRandomRoomSide(Room room)
//         {
//             var side = GetRandomSide();
//             var position = GetRandomPosOnRoomSide(room, side);
//             return new RoomSidePosition(position, side);
//         }
//
//         public Position GetRandomPosOnRoomSide(Room room, Side side)
//         {
//             if (side == Side.Top)
//             {
//                 int row = room.Position.Row;
//                 int col = m_Random.Next(1, room.Size.Width - 1) + room.Position.Col;
//                 return new Position(row, col);
//             }
//             if (side == Side.Bottom)
//             {
//                 int row = room.Position.Row + room.Size.Height - 1;
//                 int col = m_Random.Next(1, room.Size.Width - 1) + room.Position.Col;
//                 return new Position(row, col);
//             }
//             if (side == Side.Left)
//             {
//                 int row = m_Random.Next(1, room.Size.Height - 1) + room.Position.Row;
//                 int col = room.Position.Col;
//                 return new Position(row, col);
//             }
//             if (side == Side.Right)
//             {
//                 int row = m_Random.Next(1, room.Size.Height - 1) + room.Position.Row;
//                 int col = room.Position.Col + room.Size.Width - 1;
//                 return new Position(row, col);
//             }
//         
//             return Position.Empty;
//         }
//
//         public Side GetRandomSide()
//         {
//             switch (m_Random.Next(0, 4))
//             {
//                 case (0):
//                     return Side.Top;
//                 case (1):
//                     return Side.Bottom;
//                 case (2):
//                     return Side.Left;
//                 case (3):
//                     return Side.Right;
//             }
//
//             return Side.Bottom;
//         }
//     }
// }