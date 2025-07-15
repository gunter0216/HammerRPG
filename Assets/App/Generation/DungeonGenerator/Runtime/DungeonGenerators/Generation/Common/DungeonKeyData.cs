namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common
{
    public class DungeonKeyData
    {
        private readonly int m_UID;

        public int UID => m_UID;

        public DungeonKeyData(int uid)
        {
            m_UID = uid;
        }
    }
}