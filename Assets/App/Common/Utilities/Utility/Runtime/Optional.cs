namespace App.Common.Utilities.Utility.Runtime
{
    public readonly struct Optional<T>
    {
        private readonly T m_Value;
        private readonly bool m_HasValue;

        public T Value => m_Value;
        public bool HasValue => m_HasValue;

        public static Optional<T> Empty => new Optional<T>(default, false);
        
        public Optional(T value)
        {
            m_Value = value;
            m_HasValue = true;
        }
        
        public Optional(T value, bool hasValue)
        {
            m_Value = value;
            m_HasValue = hasValue;
        }
        
        public static Optional<T> Success(T result)
        {
            return new Optional<T>(result, true);
        }

        public static Optional<T> Fail()
        {
            return new Optional<T>(default, false);
        }
        
        public override string ToString()
        {
            return HasValue ? $"Optional[{m_Value}]" : "Optional.Empty";
        }

        public override bool Equals(object obj)
        {
            if (obj is not Optional<T>)
            {
                return false;
            }
            
            return Equals((Optional<T>)obj);
        }

        public bool Equals(Optional<T> other)
        {
            if (!m_HasValue && !other.m_HasValue)
            {
                return true;
            }

            if (m_HasValue && other.m_HasValue)
            {
                return m_Value.Equals(other.m_Value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return m_HasValue ? m_Value?.GetHashCode() ?? 0 : 0;
        }
    }
}