using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Game.SpriteLoaders.Runtime
{
    public interface ISpriteLoader
    {
        Optional<Sprite> Load(string key);
    }
}