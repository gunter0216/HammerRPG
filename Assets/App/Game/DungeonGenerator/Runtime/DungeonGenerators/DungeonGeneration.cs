using App.Game.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;

namespace App.Game.DungeonGenerator.Runtime.DungeonGenerators
{
    public class DungeonGeneration
    {
        private readonly Dungeon m_Dungeon;

        public Dungeon Dungeon => m_Dungeon;

        public DungeonGeneration(Dungeon dungeon)
        {
            m_Dungeon = dungeon;
        }
    }
}