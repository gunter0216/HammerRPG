using System.Collections.Generic;
using System.Linq;
using App.Common.Logger.Runtime;
using App.Menu.UI.External.FSM.States;
using UnityEngine;

namespace App.Menu.UI.External.FSM
{
    public class MenuMachine
    {
        private readonly Stack<IMenuState> m_MenuStates;

        public MenuMachine()
        {
            m_MenuStates = new Stack<IMenuState>();
        }

        public void PushState(IMenuState menuState)
        {
            if (m_MenuStates.Count > 0)
            {
                m_MenuStates.Last().Exit();
            }
            
            m_MenuStates.Push(menuState);
            menuState.Enter();
        }
        
        public void PopState()
        {
            if (m_MenuStates.Count <= 0)
            {
                HLogger.LogError("stack is empty");
                return;
            }

            var state = m_MenuStates.Pop();
            state.Exit();
            
            if (m_MenuStates.Count > 0)
            {
                m_MenuStates.Last().Enter();
            }
        }

        public IMenuState GetCurrentState()
        {
            return m_MenuStates.LastOrDefault();
        }
    }
}