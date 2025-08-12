namespace App.Common.Utilities.Utility.Runtime
{
    public class OrderedItem<T>
    {
        public T Item { get; }
        public int Order { get; }

        public OrderedItem(T item, int order)
        {
            Item = item;
            Order = order;
        }
    }
}