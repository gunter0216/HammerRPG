namespace App.Game.EcsEvent.Runtime
{
    public interface IEcsEventManager
    {
        EcsEventPool<T> GetPool<T>() where T : struct;
    }
}