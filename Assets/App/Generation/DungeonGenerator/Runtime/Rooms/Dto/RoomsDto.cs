using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.Generation.DungeonGenerator.Runtime.Rooms.Dto
{
    [JsonObject(MemberSerialization.Fields)]
    public class RoomsDto
    {
        [JsonProperty("rooms")]
        public List<RoomDto> Rooms;
    }
}