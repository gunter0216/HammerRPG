// using System.Collections.Generic;
// using App.Game.DungeonGenerator.Runtime.Utility;
// using DungeonGenerator.Matrices;
//
// namespace App.Game.DungeonGenerator.Runtime.CellFinders
// {
//     public class CellFinderMatrixCreator
//     {
//         private readonly int m_Wall;
//         private readonly int m_Empty;
//         private readonly int m_HorizontalWall;
//         private readonly int m_VerticalWall;
//
//         private Position m_From;
//         private Matrix m_Matrix;
//         private HashSet<int> m_TargetValues;
//         private bool m_MatrixWasChanged;
//         private int m_CurrentValue;
//         private Position m_FindingPosition;
//     
//         public CellFinderMatrixCreator(int wall, int empty, int horizontalWall, int verticalWall)
//         {
//             m_Wall = wall;
//             m_Empty = empty;
//             m_HorizontalWall = horizontalWall;
//             m_VerticalWall = verticalWall;
//         }
//     
//         public Position PreCalc(Matrix matrix, Position from, HashSet<int> targetValues)
//         {
//             m_FindingPosition = Position.Empty;
//             m_From = from;
//             m_TargetValues = targetValues;
//             m_Matrix = matrix;
//         
//             matrix.SetCell(from, 0);
//             m_MatrixWasChanged = true;
//             m_CurrentValue = 0;
//             while (m_MatrixWasChanged)
//             {
//                 m_MatrixWasChanged = false;
//                 for (int i = 0; i < matrix.Height; ++i)
//                 {
//                     for (int j = 0; j < matrix.Width; ++j)
//                     {
//                         if (matrix.GetCell(i, j) != m_CurrentValue)
//                         {
//                             continue;
//                         }
//
//                         Position topPos = new Position(i, j).MoveTop(1);
//                         if (CheckPosition(topPos, m_VerticalWall))
//                         {
//                             return m_FindingPosition;
//                         }
//                     
//                         Position bottomPos = new Position(i, j).MoveBottom(1);
//                         if (CheckPosition(bottomPos, m_VerticalWall))
//                         {
//                             return m_FindingPosition;
//                         }
//                     
//                         Position leftPos = new Position(i, j).MoveLeft(1);
//                         if (CheckPosition(leftPos, m_HorizontalWall))
//                         {
//                             return m_FindingPosition;
//                         }
//                     
//                         Position rightPos = new Position(i, j).MoveRight(1);
//                         if (CheckPosition(rightPos, m_HorizontalWall))
//                         {
//                             return m_FindingPosition;
//                         }
//                     }
//                 }
//
//                 m_CurrentValue += 1;
//             }
//
//             return m_FindingPosition;
//         }
//
//         private bool CheckPosition(Position position, int passableCell)
//         {
//             if (m_Matrix.IsCorrectPos(position))
//             {
//                 var value = m_Matrix.GetCell(position);
//                 if (m_TargetValues.Contains(value))
//                 {
//                     m_FindingPosition = position;
//                     return true;
//                 }
//
//                 if (value == m_Empty || value == passableCell)
//                 {
//                     m_Matrix.SetCell(position, m_CurrentValue + 1);
//                     m_MatrixWasChanged = true;
//                 }
//             }
//
//             return false;
//         }
//     }
// }