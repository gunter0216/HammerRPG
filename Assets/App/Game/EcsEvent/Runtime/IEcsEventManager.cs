using Leopotam.EcsLite;

namespace App.Game.EcsEvent.Runtime
{
    public interface IEcsEventManager
    {
        EcsEventPool<T> GetPool<T>() where T : struct;
        EcsFilter GetFilter<T>() where T : struct;
    }
}