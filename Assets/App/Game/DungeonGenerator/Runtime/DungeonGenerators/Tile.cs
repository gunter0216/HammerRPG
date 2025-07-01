namespace App.Game.DungeonGenerator.Runtime.DungeonGenerators
{
    public struct Tile
    {
        private int m_Value;
        
        public const int HorizontalWall = -4;
        public const int VerticalWall = -3;
        public const int Wall = -2;
        public const int Empty = -1;
        public const int RoomWall = 1;
        public const int Road = 2;
        public const int RoadWall = 3;
        public const int Point1 = 4;

        public int Value => m_Value;

        public static bool operator <(Tile left, Tile right)
        {
            return left.m_Value < right.m_Value;
        }

        public static bool operator >(Tile left, Tile right)
        {
            return left.m_Value > right.m_Value;
        }
    }
}