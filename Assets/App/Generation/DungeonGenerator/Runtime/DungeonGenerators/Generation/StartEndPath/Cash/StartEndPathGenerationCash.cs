using System.Collections.Generic;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.StartEndPath.Cash
{
    public class StartEndPathGenerationCash : IGenerationCash
    {
        private readonly List<DungeonRoomData> m_Path;

        public StartEndPathGenerationCash(List<DungeonRoomData> path)
        {
            m_Path = path;
        }

        public List<DungeonRoomData> Path => m_Path;
    }
}