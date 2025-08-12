using App.Common.Utilities.Utility.Runtime.Extensions;
using UnityEngine.AddressableAssets;

namespace App.Common.AssetSystem.Runtime
{
    public class StringKeyEvaluator : IKeyEvaluator
    {
        private readonly string m_Key;

        public object RuntimeKey => m_Key;

        public StringKeyEvaluator(string key)
        {
            m_Key = key;
        }
        
        public bool RuntimeKeyIsValid()
        {
            return !m_Key.IsNullOrEmpty();
        }
        
        public override bool Equals(object obj)
        {
            return Equals(obj as StringKeyEvaluator);
        }

        public bool Equals(StringKeyEvaluator other)
        {
            return other != null && m_Key == other.m_Key;
        }

        public override int GetHashCode()
        {
            return m_Key.GetHashCode();
        }
    }
}