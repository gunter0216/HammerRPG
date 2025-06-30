using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Game.Contexts;
using App.Game.EcsEvent.Runtime;
using App.Game.Inputs.Runtime;
using App.Game.Inputs.Runtime.Events;
using App.Game.States.Game;
using App.Game.Update.Runtime;
using App.Game.Update.Runtime.Attributes;
using Input = UnityEngine.Input;

namespace App.Game.Inputs.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), -1_000)]
    [RunSystem(-1_000)]
    public class InputManager : IRunSystem, IInitSystem, IInputManager
    {
        [Inject] private IEcsEventManager m_EventManager;
        
        private EcsEventPool<AxisRawEvent> m_AxisRawEventPool;
        private EcsEventPool<MousePressedEvent> m_MousePressedEventPool;

        public void Init()
        {
            m_AxisRawEventPool = m_EventManager.GetPool<AxisRawEvent>();
            m_MousePressedEventPool = m_EventManager.GetPool<MousePressedEvent>();
        }

        public void Run()
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