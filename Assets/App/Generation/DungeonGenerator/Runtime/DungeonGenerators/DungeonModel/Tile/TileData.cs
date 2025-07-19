using App.Common.Algorithms.Runtime;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel
{
    public class TileData
    {
        private string m_Id; // todo make readonly
        private Vector2Int m_Position;

        public string Id
        {
            get => m_Id;
            set { m_Id = value; }
        }

        public Vector2Int Position
        {
            get => m_Position;
            set => m_Position = value;
        }

        public TileData(string id)
        {
            Id = id;
        }
    }
}