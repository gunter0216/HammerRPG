using System;
using App.Game.DungeonGenerator.Runtime.DungeonGenerators;
using App.Game.DungeonGenerator.Runtime.Matrix;

namespace App.Game.DungeonGenerator.Runtime.Extensions
{
    public static class MatrixPrinter
    {
        public static void Print(Matrix<int> matrix)
        {
            for (int i = 0; i < matrix.Height; ++i)
            {
                for (int j = 0; j < matrix.Width; ++j)
                {
                    switch (matrix.GetCell(i, j))
                    {
                        case (Tile.Road):
                            Console.Write(' ');
                            break;
                        case (Tile.Wall):
                            Console.Write('#');
                            break;
                        case (Tile.RoomWall):
                            Console.Write('1');
                            break;
                        case (Tile.RoadWall):
                            Console.Write('@');
                            break;
                        case (Tile.VerticalWall):
                            Console.Write('|');
                            break;
                        case (Tile.HorizontalWall):
                            Console.Write('-');
                            break;
                        case (Tile.Point1):
                            Console.Write('*');
                            break;
                        // case (3):
                        //     Console.Write('6');
                        //     break;
                        default:
                            Console.Write(' ');
                            break;
                    }
                }

                Console.Write('\n');
            }
        }
    }
}