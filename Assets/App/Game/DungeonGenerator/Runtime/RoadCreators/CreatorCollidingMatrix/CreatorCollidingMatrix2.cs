using App.Game.DungeonGenerator.Runtime.DungeonGenerators;
using DungeonGenerator.Matrices;

namespace App.Game.DungeonGenerator.Runtime.RoadCreators.CreatorCollidingMatrix
{
    public class CreatorCollidingMatrix2 : ICreatorCollidingMatrix
    {
        public Matrix CreateCollidingMatrix(Matrix inputMatrix, int thickness)
        {
            Matrix matrix = new Matrix(inputMatrix.Width, inputMatrix.Height);
            matrix.Fill(Tile.Empty);
            for (int i = 0; i < inputMatrix.Height; ++i)
            {
                for (int j = 0; j < inputMatrix.Width; ++j)
                {
                    if (inputMatrix.GetCell(i, j) != Tile.Wall)
                    {
                        for (int k = i - thickness; k < i + thickness + 1; ++k)
                        {
                            for (int m = j - thickness; m < j + thickness + 1; ++m)
                            {
                                if (matrix.IsCorrectPos(k, m))
                                {
                                    matrix.SetCell(k, m, Tile.Wall);
                                }
                            }
                        }
                    }
                }
            }

            return matrix;
        }
    }
}