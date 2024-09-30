using App.Game.Player.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace App.Game.Player.Systems {
    sealed class PlayerInitSystem : IEcsInitSystem 
    {
        private readonly EcsWorldInject _world;
        private readonly EcsPoolInject<Components.Player> _playerPool;
        private readonly EcsPoolInject<PlayerInputData> _playerInputPool;
        
        private readonly EcsCustomInject<StaticData> _staticData;
        private readonly EcsCustomInject<SceneData> _sceneData;
        
        public void Init (IEcsSystems systems) {
            var playerEntity = _world.Value.NewEntity();

            ref var player = ref _playerPool.Value.Add(playerEntity);
            ref var inputData = ref _playerInputPool.Value.Add(playerEntity);
            
            var playerObj = Object.Instantiate(_staticData.Value.PlayerPrefab, _sceneData.Value.PlayerSpawnPoint);
            player.PlayerRigidbody = playerObj.GetComponent<Rigidbody2D>();
            player.MoveSpeed = _staticData.Value.PlayerMoveSpeed;
        }
    }
}