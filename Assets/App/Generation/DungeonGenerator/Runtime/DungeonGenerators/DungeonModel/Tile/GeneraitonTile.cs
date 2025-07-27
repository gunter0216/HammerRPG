namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel
{
    public class GeneraitonTile
    {
        private string m_Id; // todo make readonly

        public string Id
        {
            get => m_Id;
            set { m_Id = value; }
        }

        public GeneraitonTile(string id)
        {
            Id = id;
        }
    }
}