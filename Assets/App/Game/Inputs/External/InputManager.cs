using App.Common.Utilities.Utility.Runtime;
using App.Game.EcsEvent.Runtime;
using App.Game.Inputs.Runtime;
using App.Game.Inputs.Runtime.Events;
using Input = UnityEngine.Input;

namespace App.Game.Inputs.External
{
    public class InputManager : IUpdateSystem, IInitSystem, IInputManager
    {
        private readonly IEcsEventManager m_EventManager;
        
        private EcsEventPool<AxisRawEvent> m_AxisRawEventPool;
        private EcsEventPool<MousePressedEvent> m_MousePressedEventPool;

        public InputManager(IEcsEventManager eventManager)
        {
            m_EventManager = eventManager;
        }

        public void Init()
        {
            m_AxisRawEventPool = m_EventManager.GetPool<AxisRawEvent>();
            m_MousePressedEventPool = m_EventManager.GetPool<MousePressedEvent>();
        }

        public void OnUpdate()
        {
            m_AxisRawEventPool.Trigger(new AxisRawEvent(
                Input.GetAxisRaw("Horizontal"), 
                Input.GetAxisRaw("Vertical")));
            
            if (Input.GetMouseButtonDown(0))
            {
                m_MousePressedEventPool.Trigger(new MousePressedEvent(MouseKey.Left));
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                m_MousePressedEventPool.Trigger(new MousePressedEvent(MouseKey.Right));
            }
            
            if (Input.GetMouseButtonDown(2))
            {
                m_MousePressedEventPool.Trigger(new MousePressedEvent(MouseKey.Middle));
            }
        }
    }
}