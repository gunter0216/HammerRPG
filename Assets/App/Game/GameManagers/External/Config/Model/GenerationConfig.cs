using App.Generation.DungeonGenerator.External.Dto;

namespace App.Game.GameManagers.External.Config.Model
{
    public class GenerationConfig
    {
        private readonly DungeonGenerationConfigDto m_Generation;

        public DungeonGenerationConfigDto Generation => m_Generation;

        public GenerationConfig(DungeonGenerationConfigDto generation)
        {
            m_Generation = generation;
        }
    }
}