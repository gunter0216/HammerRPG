using App.Common.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Generation;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation
{
    public class DungeonGeneration
    {
        private readonly Dungeon m_Dungeon;
        private readonly DungeonGenerationCash m_Cash;
        private readonly DungeonGenerationConfig m_GenerationConfig;

        public Dungeon Dungeon => m_Dungeon;

        public DungeonGeneration(Dungeon dungeon, DungeonGenerationConfig generationConfig)
        {
            m_Dungeon = dungeon;
            m_GenerationConfig = generationConfig;
            m_Cash = new DungeonGenerationCash();
        }

        public bool RemoveCash<T>() where T : class, IGenerationCash
        {
            return m_Cash.RemoveCash<T>();
        }

        public bool AddCash<T>(T cash) where T : class, IGenerationCash
        {
            return m_Cash.AddCash<T>(cash);
        }

        public bool HasCash<T>() where T : class, IGenerationCash
        {
            return m_Cash.HasCash<T>();
        }

        public Optional<T> GetCash<T>() where T : class, IGenerationCash
        {
            return m_Cash.GetCash<T>();
        }

        public bool TryGetCash<T>(out T cash) where T : class, IGenerationCash
        {
            return m_Cash.TryGetCash<T>(out cash);
        }
        
        public bool HasConfig<T>() where T : class, IGenerationConfig
        {
            return m_GenerationConfig.HasConfig<T>();
        }

        public Optional<T> GetConfig<T>() where T : class, IGenerationConfig
        {
            return m_GenerationConfig.GetConfig<T>();
        }

        public bool TryGetConfig<T>(out T config) where T : class, IGenerationConfig
        {
            return m_GenerationConfig.TryGetConfig<T>(out config);
        }
    }
}