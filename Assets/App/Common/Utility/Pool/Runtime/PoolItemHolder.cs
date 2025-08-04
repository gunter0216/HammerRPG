namespace App.Common.Utility.Pool.Runtime
{
    public class PoolItemHolder<T>
    {
        public T Item { get; internal set; }
        public bool IsActive { get; internal set; }

        internal PoolItemHolder()
        {
            
        }
    }
}