using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.Utility.Runtime;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel.Generation
{
    [Serializable]
    public class DungeonGenerationConfig
    {
        private readonly IReadOnlyList<IGenerationConfig> m_GenerationConfigs;

        public IReadOnlyList<IGenerationConfig> GenerationConfigs => m_GenerationConfigs;

        public DungeonGenerationConfig(IReadOnlyList<IGenerationConfig> generationConfigs)
        {
            m_GenerationConfigs = generationConfigs;
        }
        
        public bool HasConfig<T>() where T : class, IGenerationConfig
        {
            return m_GenerationConfigs.Any(x => x is T);
        }

        public Optional<T> GetConfig<T>() where T : class, IGenerationConfig
        {
            var config = m_GenerationConfigs.FirstOrDefault(x => x is T);
            if (config == default)
            {
                return Optional<T>.Fail();
            }
            
            return Optional<T>.Success(config as T);
        }
        
        public bool TryGetConfig<T>(out T config) where T : class, IGenerationConfig
        {
            var generationCash = m_GenerationConfigs.FirstOrDefault(x => x is T);
            if (generationCash == default)
            {
                config = default;
                return false;
            }

            config = generationCash as T;
            
            return true;
        }
    }
}