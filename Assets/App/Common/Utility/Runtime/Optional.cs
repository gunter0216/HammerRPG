namespace App.Common.Utility.Runtime
{
    public struct Optional<T>
    {
        public readonly T Value { get; }
        public readonly bool HasValue { get; }

        public static Optional<T> Empty => new Optional<T>(default, false);
        
        public Optional(T value)
        {
            Value = value;
            HasValue = true;
        }
        
        public Optional(T value, bool hasValue)
        {
            Value = value;
            HasValue = hasValue;
        }
    }
}