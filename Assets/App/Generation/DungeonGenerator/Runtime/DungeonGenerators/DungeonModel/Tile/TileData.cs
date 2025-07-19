using App.Common.Algorithms.Runtime;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel
{
    public class TileData
    {
        private readonly string m_Id;
        private Vector2Int m_Position;

        public string Id => m_Id;

        public Vector2Int Position
        {
            get => m_Position;
            set => m_Position = value;
        }

        public TileData(string id)
        {
            m_Id = id;
        }
    }
}