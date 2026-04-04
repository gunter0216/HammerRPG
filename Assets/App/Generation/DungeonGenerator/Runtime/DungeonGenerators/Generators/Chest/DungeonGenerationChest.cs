using App.Common.Algorithms.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Chest
{
    public class DungeonGenerationChest
    {
        private readonly Vector2Int m_LocalPosition;
        private readonly DungeonKeyData _key;

        public Vector2Int LocalPosition => m_LocalPosition;
        public DungeonKeyData Key => _key;

        public DungeonGenerationChest(Vector2Int localPosition, DungeonKeyData key)
        {
            m_LocalPosition = localPosition;
            _key = key;
        }
    }
}