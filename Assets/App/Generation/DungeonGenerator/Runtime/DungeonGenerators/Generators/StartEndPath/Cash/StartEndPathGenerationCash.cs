using System.Collections.Generic;
using App.Generation.DungeonGenerator.Runtime.Rooms;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.StartEndPath.Cash
{
    public class StartEndPathGenerationCash : IGenerationCash
    {
        private readonly List<DungeonGenerationRoom> m_Path;

        public StartEndPathGenerationCash(List<DungeonGenerationRoom> path)
        {
            m_Path = path;
        }

        public List<DungeonGenerationRoom> Path => m_Path;
    }
}