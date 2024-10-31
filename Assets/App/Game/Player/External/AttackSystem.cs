using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.HammerDI.Runtime.Attributes;
using App.Game.Contexts;
using App.Game.EcsEvent.Runtime;
using App.Game.Inputs.Runtime.Events;
using App.Game.Player.Runtime;
using App.Game.States.Game;
using App.Game.Update.Runtime;
using App.Game.Update.Runtime.Attributes;
using App.Game.Worlds.Runtime;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace App.Game.Player.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    [RunSystem(100)]
    public class AttackSystem : IInitSystem, IRunSystem
    {
        [Inject] private IEcsEventManager m_EcsEventManager;
        [Inject] private IWorldManager m_WorldManager;
        
        private EcsEventPool<AttackEvent> m_AttackEventPool;
        private EcsFilter m_AttackEventFilter;
        
        private EcsPool<Entity> m_EntitiesPool;
        // private EcsFilter m_EntitiesFilter;

        public void Init()
        {
            // m_EntitiesFilter = m_WorldManager.GetFilter<Inc<Entity>>();
            m_EntitiesPool = m_WorldManager.GetPool<Entity>();
            
            m_AttackEventPool = m_EcsEventManager.GetPool<AttackEvent>();
            m_AttackEventFilter = m_EcsEventManager.GetFilter<AttackEvent>();
        }

        public void Run()
        {
            foreach (var i in m_AttackEventFilter)
            {
                ref var attackEvent = ref m_AttackEventPool.Get(i);
                ref var entity = ref m_EntitiesPool.Get(attackEvent.EntityId);

                // todo
            }
        }
    }
}