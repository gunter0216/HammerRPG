using Leopotam.EcsLite;

namespace App.Game.Worlds.Runtime
{
    public interface IWorldManager
    {
        EcsWorld GetWorld(string name = null);
        EcsSystems GetSystems();
    }
}