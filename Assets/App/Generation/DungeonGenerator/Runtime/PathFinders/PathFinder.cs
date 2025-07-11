// using System;
// using System.Collections.Generic;
// using App.Game.DungeonGenerator.Runtime.PathFinders.PathBuilders;
// using App.Game.DungeonGenerator.Runtime.Utility;
// using DungeonGenerator.Matrices;
//
// namespace App.Game.DungeonGenerator.Runtime.PathFinders
// {
//     public class PathFinder : IPathFinder
//     {
//         private readonly int m_Wall;
//         private readonly int m_Empty;
//         private readonly int m_HorizontalWall;
//         private readonly int m_VerticalWall;
//         private readonly int[] m_InputCellValues;
//         private Position m_From;
//         private Position m_To;
//         private Matrix m_Matrix;
//         private PathFinderMatrixCreator m_PathFinderMatrixCreator;
//         private IPathBuilder m_PathBuilder;
//
//         public PathFinder(int wall, int empty, int horizontalWall, int verticalWall)
//         {
//             m_Wall = wall;
//             m_Empty = empty;
//             m_HorizontalWall = horizontalWall;
//             m_VerticalWall = verticalWall;
//             m_InputCellValues = new[] { m_Wall, m_Empty, m_HorizontalWall, m_VerticalWall };
//             m_PathFinderMatrixCreator = new PathFinderMatrixCreator(
//                 wall, 
//                 empty, 
//                 horizontalWall, 
//                 verticalWall);
//             m_PathBuilder = new PathBuilder();
//         }
//     
//         public Optional<List<Position>> FindPath(
//             Matrix inputMatrix,
//             Position from,
//             Position to)
//         {
//             m_From = from;
//             m_To = to;
//             m_Matrix = new Matrix(inputMatrix);
//         
//             if (!IsCorrectMatrix(inputMatrix))
//             {
//                 return Optional<List<Position>>.Empty();
//             }
//         
//             m_PathFinderMatrixCreator.PreCalc(m_Matrix, m_From, m_To);
//             return m_PathBuilder.BuildPath(m_Matrix, m_From, m_To, m_InputCellValues);
//         }
//
//         private bool IsCorrectMatrix(Matrix inputMatrix)
//         {
//             return inputMatrix.ContainsOnly(m_InputCellValues);
//         }
//
//         private void PrintDungeon()
//         {
//             for (int i = 0; i < m_Matrix.Height; ++i)
//             {
//                 for (int j = 0; j < m_Matrix.Width; ++j)
//                 {
//                     string printString = "";
//                     if (new Position(i, j) == m_From)
//                     {
//                         Console.Write("___");
//                         continue;
//                     }
//                     if (new Position(i, j) == m_To)
//                     {
//                         Console.Write("---");
//                         continue;
//                     }
//                     var cellValue = m_Matrix.GetCell(i, j);
//                     if (cellValue == m_Wall)
//                     {
//                         printString = String.Format("## ");
//                     } 
//                     else if (cellValue == m_Empty)
//                     {
//                         printString = "   ";
//                     }
//                     else
//                     {
//                         printString = String.Format("{0:d2} ", m_Matrix.GetCell(i, j));
//                     }
//                     Console.Write(printString);
//                 }
//
//                 Console.Write('\n');
//             }
//         }
//     }
// }