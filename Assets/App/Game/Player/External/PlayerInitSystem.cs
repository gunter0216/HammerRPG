using App.Common.AssetSystem.Runtime;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.HammerDI.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Game.Contexts;
using App.Game.Player.Runtime;
using App.Game.States.Game;
using App.Game.Worlds.Runtime;
using UnityEngine;

namespace App.Game.Player.External 
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    sealed class PlayerInitSystem : IInitSystem
    {
        private const string m_PlayerAssetKey = "Player";
        private const float m_DefaultMoveSpeed = 5.0f;
        
        [Inject] private IWorldManager m_WorldManager;
        [Inject] private IAssetManager m_AssetManager;

        public void Init()
        {
            var entityView = m_AssetManager.InstantiateSync<EntityView>(new StringKeyEvaluator(m_PlayerAssetKey));
            if (!entityView.HasValue)
            {
                HLogger.LogError("cant create player");
                return;
            }
            
            var world = m_WorldManager.GetWorld();
            var playerEntity = world.NewEntity();

            var entities = world.GetPool<Entity>();
            var playerPool = world.GetPool<PlayerComponent>();
            ref var entity = ref entities.Add(playerEntity);
            ref var playerComponent = ref playerPool.Add(playerEntity);
            
            entity.View = entityView.Value;
            entity.View.Weapon.gameObject.SetActive(false);
            entity.MoveSpeed = m_DefaultMoveSpeed;
        }
    }
}