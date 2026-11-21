// using System.Collections.Generic;
// using DungeonGenerator.JsonControllers;
// using DungeonGenerator.Rooms.Templates;
// using DungeonGenerator.Utility.Loggers;
//
// namespace DungeonGenerator.Rooms
// {
//     public class RoomController
//     {
//         private IJsonController m_JsonController;
//
//         private Dictionary<string, RoomTemplate> m_RoomConfigs = new();
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
//             var roomTemplate = m_JsonController.ReadJson<RoomTemplate>(path);
//             if (!roomTemplate.HasValue)
//             {
//                 Logger.Log($"Cant read json {path}");
//                 return;
//             }
//             
//             m_RoomConfigs.Add(roomTemplate.Value.Name, roomTemplate.Value);
//         }
//     }
// }