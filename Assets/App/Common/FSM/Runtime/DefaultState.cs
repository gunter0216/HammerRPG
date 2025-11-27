using System;
using System.Collections.Generic;
using App.Common.Utilities.Utility.Runtime;

namespace App.Common.FSM.Runtime
{
    public class DefaultState : IState
    {
        private readonly int m_Stage;
        private readonly List<Func<bool>> m_Predicates;
        private List<IInitSystem> m_Systems;
        private List<IPostInitSystem> m_PostInitSystems;

        public DefaultState(int stage, List<Func<bool>> predicates = null)
        {
            m_Predicates = predicates;
            m_Stage = stage;
        }

        public void SetSystems(List<IInitSystem> systems, List<IPostInitSystem> postInitSystems)
        {
            m_Systems = systems;
            m_PostInitSystems = postInitSystems;
        }

        public int GetStage()
        {
            return m_Stage;
        }

        public void SyncRun()
        {
            for (int i = 0; i < m_Systems.Count; ++i)
            {
                m_Systems[i].Init();
            }
            
            for (int i = 0; i < m_PostInitSystems.Count; ++i)
            {
                m_PostInitSystems[i].PostInit();
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