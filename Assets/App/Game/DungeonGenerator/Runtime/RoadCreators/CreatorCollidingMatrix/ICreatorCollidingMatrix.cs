using DungeonGenerator.Matrices;

namespace App.Game.DungeonGenerator.Runtime.RoadCreators.CreatorCollidingMatrix
{
    public interface ICreatorCollidingMatrix
    { 
        Matrix CreateCollidingMatrix(Matrix inputMatrix, int thickness);
    }
}