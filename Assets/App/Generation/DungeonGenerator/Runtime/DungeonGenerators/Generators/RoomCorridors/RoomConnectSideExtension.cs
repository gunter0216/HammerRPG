using App.Common.Logger.Runtime;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Corridors
{
    public static class RoomConnectSideExtension
    {
        public static RoomConnectSide Revert(this RoomConnectSide side)
        {
            if (side == RoomConnectSide.Bottom)
            {
                return RoomConnectSide.Top;
            }
            
            if (side == RoomConnectSide.Top)
            {
                return RoomConnectSide.Bottom;
            }
            
            if (side == RoomConnectSide.Left)
            {
                return RoomConnectSide.Right;
            }
            
            if (side == RoomConnectSide.Right)
            {
                return RoomConnectSide.Left;
            }

            HLogger.LogError("error");
            
            return RoomConnectSide.Left;
        }
    }
}