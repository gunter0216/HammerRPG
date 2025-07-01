using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Game.DungeonGenerator.Runtime.Rooms.Dto
{
    [JsonObject(MemberSerialization.Fields)]
    public class RoomDto
    {
        public string Name;
        public string LookAt;
        public bool HorizontalRotate;
        public bool VerticalRotate;
        public List<string> Matrix;
    }
}