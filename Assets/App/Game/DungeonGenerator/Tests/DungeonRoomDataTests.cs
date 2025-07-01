using App.Game.DungeonGenerator.Runtime.Rooms;
using NUnit.Framework;
using UnityEngine;

namespace App.Game.DungeonGenerator.Tests
{
    public class DungeonRoomDataTests
    {
        [Test]
        public void IntersectTest()
        {
            var room1 = new DungeonRoomData(new Vector2Int(1, 1), new Vector2Int(1, 1));
            var room2 = new DungeonRoomData(new Vector2Int(1, 1), new Vector2Int(1, 1));
        }
    }
}