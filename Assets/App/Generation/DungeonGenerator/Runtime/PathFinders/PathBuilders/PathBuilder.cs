// using System;
// using System.Collections.Generic;
// using App.Game.DungeonGenerator.Runtime.Utility;
// using DungeonGenerator.Matrices;
//
// namespace App.Game.DungeonGenerator.Runtime.PathFinders.PathBuilders
// {
//     public class PathBuilder : IPathBuilder
//     {
//         private readonly Position[] m_Offsets = new Position[]
//         {
//             new Position( 0, 1 ), 
//             new Position( 0, -1 ), 
//             new Position( 1, 0 ), 
//             new Position( -1, 0 )
//         };
//     
//         private Matrix m_Matrix;
//         private Position m_To;
//         private Position m_From;
//         private HashSet<int> m_IgnoredCellValues;
//
//         public PathBuilder()
//         {
//             // do nothing
//         }
//
//         public Optional<List<Position>> BuildPath(Matrix matrix, Position from, Position to, params int[] ignoredCellValues)
//         {
//             m_Matrix = matrix;
//             m_From = from;
//             m_To = to;
//             m_IgnoredCellValues = new HashSet<int>(ignoredCellValues);
//         
//             List<Position> buildPath = new List<Position> { m_To };
//             Position currentPos = m_To;
//             m_Matrix.SetCell(m_To, Int32.MaxValue);
//             while (currentPos != m_From)
//             {
//                 currentPos = GetCellWithMinValueAround(currentPos);
//                 if (currentPos.IsEmpty())
//                 {
//                     return Optional<List<Position>>.Empty();
//                 }
//
//                 buildPath.Add(currentPos);
//             }
//
//             return new Optional<List<Position>>(buildPath);
//         }
//
//         private Position GetCellWithMinValueAround(Position pos)
//         {
//             Position cellWithMinValue = Position.Empty;
//             int minValue = m_Matrix.GetCell(pos);
//             foreach (var offset in m_Offsets)
//             {
//                 Position newPos = pos + offset;
//                 if (m_Matrix.IsCorrectPos(newPos))
//                 {
//                     int cellValue = m_Matrix.GetCell(newPos);
//                     if (!m_IgnoredCellValues.Contains(cellValue) && 
//                         cellValue < minValue)
//                     {
//                         minValue = cellValue;
//                         cellWithMinValue = newPos;
//                     }
//                 }
//             }
//
//             return cellWithMinValue;
//         }
//     }
// }