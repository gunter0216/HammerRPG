using System;
using System.Collections.Generic;
using App.Game.GameManagers.External.Room;
using Newtonsoft.Json;

namespace App.Generation.DungeonCreator.Runtime
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class DungeonData
    {
        [JsonProperty("rooms")] 
        private List<RoomData> m_Rooms;
        
        [JsonProperty("start_room")]
        private int m_StartRoom;
        
        [JsonProperty("end_room")]
        private int m_EndRoom;

        public List<RoomData> Rooms
        {
            get => m_Rooms;
            set => m_Rooms = value;
        }

        public int StartRoom
        {
            get => m_StartRoom;
            set => m_StartRoom = value;
        }

        public int EndRoom
        {
            get => m_EndRoom;
            set => m_EndRoom = value;
        }
    }
}