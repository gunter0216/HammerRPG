using Leopotam.EcsLite;

namespace App.Game.Worlds.Runtime
{
    public interface IWorldManager
    {
        EcsWorld GetWorld(string name = null);
        EcsSystems GetSystems();
        EcsPool<T> GetPool<T>(string worldName = null) where T : struct;
        EcsFilter GetFilter<T>(string worldName = null) where T : struct;
    }
}