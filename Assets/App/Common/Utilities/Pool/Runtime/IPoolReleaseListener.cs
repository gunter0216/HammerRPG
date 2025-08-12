namespace App.Common.Utility.Pool.Runtime
{
    public interface IPoolReleaseListener
    {
        void BeforeReturnInPool();
    }
}