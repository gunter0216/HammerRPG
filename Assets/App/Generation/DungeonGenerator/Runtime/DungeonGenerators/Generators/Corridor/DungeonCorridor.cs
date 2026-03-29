using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.SquarePartition;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridor
{
    public class DungeonCorridor
    {
        private readonly SquareArea _area;
        private readonly RoomConnectSide _side;

        public DungeonCorridor(SquareArea area, RoomConnectSide side)
        {
            _area = area;
            _side = side;
        }

        public SquareArea Area => _area;
        public RoomConnectSide Side => _side;
        public bool IsHorizontal => _side == RoomConnectSide.Left || _side == RoomConnectSide.Right;
    }
}