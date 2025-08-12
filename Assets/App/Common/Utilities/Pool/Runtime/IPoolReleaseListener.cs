namespace App.Common.Utilities.Pool.Runtime
{
    public interface IPoolReleaseListener
    {
        void BeforeReturnInPool();
    }
}