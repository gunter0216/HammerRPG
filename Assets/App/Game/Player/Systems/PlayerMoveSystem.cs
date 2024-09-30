using App.Game.Player.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace App.Game.Player.Systems
{
    public class PlayerMoveSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<Components.Player, PlayerInputData>> _filter;
        private readonly EcsPoolInject<Components.Player> _player;
        private readonly EcsPoolInject<PlayerInputData> _playerInput;

        public void Run(IEcsSystems systems)
        {
            foreach (var i in _filter.Value)
            {
                ref var player = ref _player.Value.Get(i);
                ref var input = ref _playerInput.Value.Get(i);

                var velocity = input.MoveInput.normalized * player.MoveSpeed;
                player.PlayerRigidbody.velocity = velocity;
            }
        }
    }
}