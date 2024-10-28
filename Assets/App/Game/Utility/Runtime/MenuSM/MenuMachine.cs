using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.Logger.Runtime;

namespace App.Game.Utility.Runtime.MenuSM
{
    public class MenuMachine
    {
        private readonly Stack<IMenuState> m_MenuStates;

        private Action<IMenuState> m_PushAction;
        private Action<IMenuState> m_PopAction;

        public MenuMachine(
            Action<IMenuState> pushAction = null, 
            Action<IMenuState> popAction = null)
        {
            m_PushAction = pushAction;
            m_PopAction = popAction;
            m_MenuStates = new Stack<IMenuState>();
        }

        public void PushState(IMenuState menuState)
        {
            if (m_MenuStates.Count > 0)
            {
                m_MenuStates.Peek().Exit();
            }
            
            m_MenuStates.Push(menuState);
            menuState.Enter();
            m_PushAction?.Invoke(menuState);
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
                m_MenuStates.Peek().Enter();
            }
            
            m_PopAction?.Invoke(state);
        }

        public int GetCountInStack()
        {
            return m_MenuStates.Count;
        }

        public IMenuState GetCurrentState()
        {
            return m_MenuStates.LastOrDefault();
        }
    }
}