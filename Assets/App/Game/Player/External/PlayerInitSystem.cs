using App.Common.AssetSystem.Runtime;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.HammerDI.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Game.Contexts;
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
            var playerObject = m_AssetManager.InstantiateSync<Rigidbody2D>(new StringKeyEvaluator(m_PlayerAssetKey));
            if (!playerObject.HasValue)
            {
                HLogger.LogError("cant create player");
                return;
            }
            
            var world = m_WorldManager.GetWorld();
            var playerEntity = world.NewEntity();

            var entities = world.GetPool<Entity>();
            ref var player = ref entities.Add(playerEntity);
            
            player.PlayerRigidbody = playerObject.Value.gameObject.GetComponent<Rigidbody2D>();
            player.MoveSpeed = m_DefaultMoveSpeed;
        }
    }
}