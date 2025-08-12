using System.Collections.Generic;
using System.Linq;
using App.Common.Utilities.Utility.Runtime;

namespace App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation
{
    public class DungeonGenerationCash
    {
        private readonly List<IGenerationCash> m_Cash;

        public DungeonGenerationCash()
        {
            m_Cash = new List<IGenerationCash>();
        }
        
        public bool RemoveCash<T>() where T : class, IGenerationCash
        {
            for (int i = 0; i < m_Cash.Count; ++i)
            {
                if (m_Cash[i] is T)
                {
                    m_Cash.RemoveAt(i);
                    return true;
                }
            }
            
            return false;
        }
        
        public bool AddCash<T>(T cash) where T : class, IGenerationCash
        {
            if (HasCash<T>())
            {
                return false;
            }
            
            m_Cash.Add(cash);
            return true;
        }
        
        public bool HasCash<T>() where T : class, IGenerationCash
        {
            return m_Cash.Any(x => x is T);
        }

        public Optional<T> GetCash<T>() where T : class, IGenerationCash
        {
            var cash = m_Cash.FirstOrDefault(x => x is T);
            if (cash == default)
            {
                return Optional<T>.Fail();
            }
            
            return Optional<T>.Success(cash as T);
        }
        
        public bool TryGetCash<T>(out T cash) where T : class, IGenerationCash
        {
            var generationCash = m_Cash.FirstOrDefault(x => x is T);
            if (generationCash == default)
            {
                cash = default;
                return false;
            }

            cash = generationCash as T;
            
            return true;
        }
    }
}