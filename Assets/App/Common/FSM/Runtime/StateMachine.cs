using System.Collections;
using System.Collections.Generic;

namespace App.Common.FSM.Runtime
{
    public class StateMachine : IStateMachine
    {
        private readonly List<IState> m_States;

        public StateMachine()
        {
            m_States = new List<IState>();
        }

        public void AddState(IState state)
        {
            m_States.Add(state);
        }

        public IEnumerator Run()
        {
            for (int i = 0; i < m_States.Count; ++i)
            {
                
            }
        }
    }
}