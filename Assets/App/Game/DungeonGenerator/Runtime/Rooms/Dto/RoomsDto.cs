using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Game.DungeonGenerator.Runtime.Rooms.Dto
{
    [JsonObject(MemberSerialization.Fields)]
    public class RoomsDto
    {
        [JsonProperty("rooms")]
        public List<RoomDto> Rooms;
    }
}