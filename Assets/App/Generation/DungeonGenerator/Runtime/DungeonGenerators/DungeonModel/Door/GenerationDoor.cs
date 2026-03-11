using App.Common.Algorithms.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Door
{
    public class GenerationDoor
    {
        private readonly Vector2Int m_LocalPosition;
        private readonly DungeonKeyData m_RequiredKey;

        public Vector2Int LocalPosition => m_LocalPosition;

        public DungeonKeyData RequiredKey => m_RequiredKey;
        public bool IsRequiredKey => m_RequiredKey?.UID != null;

        public GenerationDoor(Vector2Int localPosition, DungeonKeyData requiredKey)
        {
            m_LocalPosition = localPosition;
            m_RequiredKey = requiredKey;
        }
    }
}