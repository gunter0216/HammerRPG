namespace App.Common.Algorithms.Runtime
{
    public readonly struct RectInt
    {
        private readonly Vector2Int _position;
        private readonly Vector2Int _size;

        public Vector2Int Position => _position;

        public Vector2Int Size => _size;

        public int Width => _size.X;
        public int Height => _size.Y;
        
        public RectInt(Vector2Int position, Vector2Int size)
        {
            _position = position;
            _size = size;
        }
    }
}