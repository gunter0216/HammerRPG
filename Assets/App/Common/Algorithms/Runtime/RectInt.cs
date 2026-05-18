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

        public RectInt(int positionX, int positionY, int sizeX, int sizeY)
        {
            _position = new Vector2Int(positionX, positionY);
            _size = new Vector2Int(sizeX, sizeY);
        }

        public bool Overlaps(RectInt rect2)
        {
            return _position.X < rect2._position.X + rect2.Width &&
                   _position.X + Width > rect2._position.X &&
                   _position.Y < rect2._position.Y + rect2.Height &&
                   _position.Y + Height > rect2._position.Y;
        }
    }
}