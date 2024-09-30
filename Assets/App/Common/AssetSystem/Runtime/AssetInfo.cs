using UnityEngine;

namespace App.Common.AssetSystem.Runtime
{
    public class AssetInfo
    {
        public Object Asset { get; set; }
        public float UsedLastTime { get; set; }
        public bool MarkToUnload { get; set; }
    }
}