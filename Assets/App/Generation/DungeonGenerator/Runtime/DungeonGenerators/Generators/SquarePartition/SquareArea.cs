using App.Common.Algorithms.Runtime;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SquarePartition
{
    public class SquareArea
    {
        private readonly Vector2Int _size;
        private readonly Vector2Int _position;

        public Vector2Int Size => _size;

        public Vector2Int Position => _position;

        public SquareArea(Vector2Int position, Vector2Int size)
        {
            _size = size;
            _position = position;
        }
    }
}