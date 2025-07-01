// using System.Collections.Generic;
// using DungeonGenerator.JsonControllers;
// using DungeonGenerator.Rooms.Dto;
// using DungeonGenerator.Utility.Loggers;
//
// namespace DungeonGenerator.Rooms
// {
//     public class RoomController
//     {
//         private IJsonController m_JsonController;
//
//         private Dictionary<string, RoomDto> m_RoomConfigs = new();
//         
//         public RoomController()
//         {
//             m_JsonController = new JsonController();
//         }
//
//         public void ReadRoomConfigs()
//         {
//             ReadRoomConfig("../DungeonGenerator/Rooms/Content/Room0.json");
//             ReadRoomConfig("../DungeonGenerator/Rooms/Content/Room1.json");
//         }
//
//         private void ReadRoomConfig(string path)
//         {
//             var roomDto = m_JsonController.ReadJson<RoomDto>(path);
//             if (!roomDto.HasValue)
//             {
//                 Logger.Log($"Cant read json {path}");
//                 return;
//             }
//             
//             m_RoomConfigs.Add(roomDto.Value.Name, roomDto.Value);
//         }
//     }
// }