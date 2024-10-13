using System;
using System.Collections;
using System.Collections.Generic;
using App.Game;

namespace App.Common.FSM.Runtime
{
    public class DefaultStage : IStage
    {
        private readonly string m_Name;
        private readonly List<Func<bool>> m_Predicates;
        private List<IInitSystem> m_Systems;

        public DefaultStage(Type type, List<Func<bool>> predicates = null)
        {
            m_Predicates = predicates;
            m_Name = type.Name;
        }

        public void SetSystems(List<IInitSystem> systems)
        {
            m_Systems = systems;
        }

        public string GetName()
        {
            return m_Name;
        }

        public void SyncRun()
        {
            for (int i = 0; i < m_Systems.Count; ++i)
            {
                m_Systems[i].Init();
            }
        }
        
        public bool IsPredicatesCompleted()
        {
            foreach (var predicate in m_Predicates)
            {
                if (!predicate.Invoke())
                {
                    return false;
                }
            }

            return true;
        }
    }
}