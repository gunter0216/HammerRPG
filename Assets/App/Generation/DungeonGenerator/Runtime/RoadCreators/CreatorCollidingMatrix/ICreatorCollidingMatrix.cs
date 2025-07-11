namespace App.Generation.DungeonGenerator.Runtime.RoadCreators.CreatorCollidingMatrix
{
    public interface ICreatorCollidingMatrix
    { 
        Matrix.Matrix CreateCollidingMatrix(Matrix.Matrix inputMatrix, int thickness);
    }
}