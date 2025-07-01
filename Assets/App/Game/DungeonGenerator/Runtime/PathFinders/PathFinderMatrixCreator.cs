// using App.Game.DungeonGenerator.Runtime.Utility;
// using DungeonGenerator.Matrices;
//
// namespace App.Game.DungeonGenerator.Runtime.PathFinders
// {
//     public class PathFinderMatrixCreator
//     {
//         // private readonly Position[] m_Offsets = new Position[]
//         // {
//         //     new Position( 0, 1 ), 
//         //     new Position( 0, -1 ), 
//         //     new Position( 1, 0 ), 
//         //     new Position( -1, 0 )
//         // };
//     
//         private readonly int m_Wall;
//         private readonly int m_Empty;
//         private readonly int m_HorizontalWall;
//         private readonly int m_VerticalWall;
//
//         private Position m_From;
//         private Position m_To;
//         private Matrix m_Matrix;
//         private bool m_MatrixWasChanged;
//         private int m_CurrentValue;
//     
//         public PathFinderMatrixCreator(int wall, int empty, int horizontalWall, int verticalWall)
//         {
//             m_Wall = wall;
//             m_Empty = empty;
//             m_HorizontalWall = horizontalWall;
//             m_VerticalWall = verticalWall;
//         }
//     
//         public void PreCalc(Matrix matrix, Position from, Position to)
//         {
//             m_From = from;
//             m_To = to;
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
//                             return;
//                         }
//                     
//                         Position bottomPos = new Position(i, j).MoveBottom(1);
//                         if (CheckPosition(bottomPos, m_VerticalWall))
//                         {
//                             return;
//                         }
//                     
//                         Position leftPos = new Position(i, j).MoveLeft(1);
//                         if (CheckPosition(leftPos, m_HorizontalWall))
//                         {
//                             return;
//                         }
//                     
//                         Position rightPos = new Position(i, j).MoveRight(1);
//                         if (CheckPosition(rightPos, m_HorizontalWall))
//                         {
//                             return;
//                         }
//                     }
//                 }
//
//                 m_CurrentValue += 1;
//             }
//         }
//
//         private bool CheckPosition(Position position, int passableCell)
//         {
//             if (m_Matrix.IsCorrectPos(position))
//             {
//                 if (position == m_To)
//                 {
//                     return true;
//                 }
//                     
//                 var value = m_Matrix.GetCell(position);
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
//
//
// // foreach (var offset in m_Offsets)
// // {
// //     Position newPos = new Position(i + offset.Row, j + offset.Col);
// //     if (m_Matrix.IsCorrectPos(newPos))
// //     {
// //         if (newPos == m_To)
// //         {
// //             return;
// //         }
// //         
// //         if (m_Matrix.GetCell(newPos) == m_Empty)
// //         {
// //             m_Matrix.SetCell(newPos, currentValue + 1);
// //             matrixWasChanged = true;
// //         }
// //     }
// // }