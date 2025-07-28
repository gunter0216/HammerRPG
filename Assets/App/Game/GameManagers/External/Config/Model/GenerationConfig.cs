using App.Generation.DungeonGenerator.External.Dto;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Generation;

namespace App.Game.GameManagers.External.Config.Model
{
    public class GenerationConfig
    {
        private readonly DungeonGenerationConfig m_Generation;

        public DungeonGenerationConfig Generation => m_Generation;

        public GenerationConfig(DungeonGenerationConfig generation)
        {
            m_Generation = generation;
        }
    }
}