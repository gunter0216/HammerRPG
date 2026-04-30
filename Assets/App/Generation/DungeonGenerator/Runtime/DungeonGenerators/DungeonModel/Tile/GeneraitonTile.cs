namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel
{
    public class GeneraitonTile
    {
        private DungeonTile _tile;

        public DungeonTile Id
        {
            get => _tile;
            set { _tile = value; }
        }

        public GeneraitonTile(DungeonTile id)
        {
            Id = id;
        }
    }
}