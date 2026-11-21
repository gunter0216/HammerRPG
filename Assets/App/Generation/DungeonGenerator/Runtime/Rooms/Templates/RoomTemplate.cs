using System.Collections.Generic;

namespace App.Generation.DungeonGenerator.Runtime.Rooms.Templates
{
    public class RoomTemplate
    {
        public string Name;
        public string LookAt;
        public bool HorizontalRotate;
        public bool VerticalRotate;
        public List<string> Matrix;
    }
}