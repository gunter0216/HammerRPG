namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common
{
    public class DungeonKeyCreator
    {
        private static int m_Index;
        
        public DungeonKeyData Create()
        {
            var uid = m_Index++;
            var key = new DungeonKeyData(uid);
            return key;
        }
    }
}