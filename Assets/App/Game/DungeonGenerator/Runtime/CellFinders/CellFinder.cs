// using System.Collections.Generic;
// using App.Game.DungeonGenerator.Runtime.DungeonGenerators;
// using App.Game.DungeonGenerator.Runtime.PathFinders.PathBuilders;
// using App.Game.DungeonGenerator.Runtime.Utility;
// using App.Game.DungeonGenerator.Runtime.Utility.Loggers;
// using DungeonGenerator.Matrices;
//
// namespace App.Game.DungeonGenerator.Runtime.CellFinders
// {
//     public class CellFinder : ICellFinder
//     {
//         private readonly int m_Wall;
//         private readonly int m_Empty;
//         private readonly int m_HorizontalWall;
//         private readonly int m_VerticalWall;
//         private readonly int[] m_InputCellValues;
//         private Position m_From;
//         private int m_TargetValue;
//         private HashSet<int> m_TargetValues;
//         private Matrix m_Matrix;
//         private CellFinderMatrixCreator m_CellFinderMatrixCreator;
//         private IPathBuilder m_PathBuilder;
//
//         public CellFinder(int wall, int empty, int horizontalWall, int verticalWall)
//         {
//             m_Wall = wall;
//             m_Empty = empty;
//             m_HorizontalWall = horizontalWall;
//             m_VerticalWall = verticalWall;
//             m_InputCellValues = new[] { m_Wall, m_Empty, m_HorizontalWall, m_VerticalWall };
//             m_CellFinderMatrixCreator = new CellFinderMatrixCreator(
//                 wall, 
//                 empty, 
//                 horizontalWall, 
//                 verticalWall);
//             m_PathBuilder = new PathBuilder();
//         }
//
//         public Optional<List<Position>> FindPath(Matrix inputMatrix, Position from, int targetValue)
//         {
//             m_TargetValues = new HashSet<int>(targetValue);
//             return FindPath(inputMatrix, from);
//         }
//
//         public Optional<List<Position>> FindPath(Matrix inputMatrix, Position from, params int[] targetValues)
//         {
//             m_TargetValues = new HashSet<int>(targetValues);
//             return FindPath(inputMatrix, from);
//         }
//
//         private Optional<List<Position>> FindPath(Matrix inputMatrix, Position from)
//         {
//             m_From = from;
//             m_Matrix = new Matrix(inputMatrix);
//         
//             if (!IsCorrectMatrix(inputMatrix))
//             {
//                 Logger.Log("Incorrect matrix");
//                 return Optional<List<Position>>.Empty();
//             }
//         
//             var position = m_CellFinderMatrixCreator.PreCalc(m_Matrix, m_From, m_TargetValues);
//             if (position.IsEmpty())
//             {
//                 Logger.Log("Dont find cell value");
//                 return Optional<List<Position>>.Empty();
//             }
//         
//             m_Matrix.SetCell(position, Tile.Point1);
//             // Logger.Log("aliluya");
//             // MatrixPrinter.Print(m_Matrix);
//         
//             // Logger.Log(position);
//         
//             return m_PathBuilder.BuildPath(m_Matrix, m_From, position, m_InputCellValues);
//         }
//
//         private bool IsCorrectMatrix(Matrix inputMatrix)
//         {
//             return inputMatrix.ContainsOnly(m_InputCellValues);
//         }
//     }
// }