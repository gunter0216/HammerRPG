using App.Common.Utility.Runtime.Extensions;
using UnityEngine.AddressableAssets;

namespace App.Common.AssetSystem.Runtime
{
    public class StringKeyEvaluator : IKeyEvaluator
    {
        private readonly string _key;

        public object RuntimeKey => _key;

        public StringKeyEvaluator(string key)
        {
            _key = key;
        }
        
        public bool RuntimeKeyIsValid()
        {
            return _key.IsNullOrEmpty();
        }
    }
}