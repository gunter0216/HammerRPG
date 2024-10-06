using System.Collections.Generic;
using System.Linq;
using App.Menu.UI.External.FSM.States;

namespace App.Menu.UI.External.FSM
{
    public class MenuFSM
    {
        private readonly Stack<IMenuState> m_MenuStates;

        public MenuFSM()
        {
            m_MenuStates = new Stack<IMenuState>();
        }

        public void PushState(IMenuState menuState)
        {
            m_MenuStates.Push(menuState);
        }
        
        public IMenuState PopState()
        {
            return m_MenuStates.Pop();
        }

        public IMenuState GetCurrentState()
        {
            return m_MenuStates.LastOrDefault();
        }
    }
}