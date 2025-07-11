// using App.Game.DungeonGenerator.Runtime.Utility;
// using App.Game.DungeonGenerator.Runtime.Utility.Loggers;
// using DungeonGenerator.Matrices;
//
// namespace App.Game.DungeonGenerator.Runtime.Extensions
// {
//     public static class MatrixExtensions
//     {
//         public static void SetLine<T>(this Matrix<T> matrix, Position from, Position to, T value)
//         {
//             if (!matrix.IsCorrectPos(from))
//             {
//                 Logger.Log($"Incorrect position from {from}");
//                 return;
//             }
//         
//             if (!matrix.IsCorrectPos(to))
//             {
//                 Logger.Log($"Incorrect position to {to}");
//                 return;
//             }
//         
//             if (from.Col == to.Col)
//             {
//                 if (from.Row < to.Row)
//                 {
//                     for (int i = from.Row; i < to.Row + 1; ++i)
//                     {
//                         Position pos = new Position(i, to.Col);
//                         matrix.SetCell(pos, value);
//                     }
//                 }
//                 else
//                 {
//                     for (int i = to.Row; i < from.Row + 1; ++i)
//                     {
//                         Position pos = new Position(i, to.Col);
//                         matrix.SetCell(pos, value);
//                     }
//                 }
//             
//             }
//             else if (from.Row == to.Row)
//             {
//                 if (from.Col < to.Col)
//                 {
//                     for (int i = from.Col; i < to.Col + 1; ++i)
//                     {
//                         Position pos = new Position(to.Row, i);
//                         matrix.SetCell(pos, value);
//                     }
//                 }
//                 else
//                 {
//                     for (int i = to.Col; i < from.Col + 1; ++i)
//                     {
//                         Position pos = new Position(to.Row, i);
//                         matrix.SetCell(pos, value);
//                     }
//                 }
//             }
//             else
//             {
//                 Logger.Log("Incorrect line. Cant be diagonal");
//             }
//         }
//
//         public static Position GetPositionFromSide<T>(this Matrix<T> matrix, Side side)
//         {
//             switch (side)
//             {
//                 case (Side.TopLeft):
//                     return new Position(0, 0);
//                 case (Side.TopRight):
//                     return new Position(0, matrix.Width - 1);
//                 case (Side.BottomLeft):
//                     return new Position(matrix.Height - 1, 0);
//                 case (Side.BottomRight):
//                     return new Position(matrix.Height - 1, matrix.Width - 1);
//                 case (Side.Top):
//                     return new Position(0, matrix.Width / 2);
//                 case (Side.Bottom):
//                     return new Position(matrix.Height - 1, matrix.Width / 2);
//                 case (Side.Right):
//                     return new Position(matrix.Height / 2, matrix.Width - 1);
//                 case (Side.Left):
//                     return new Position(matrix.Height / 2, 0);
//                 default:
//                     Logger.Log("Dont realization side");
//                     return Position.Empty;
//             }
//         }
//     }
// }