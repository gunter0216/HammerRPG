using UnityEngine;

namespace App.Game.IconLoaders.Runtime
{
    public interface IIconLoader
    {
        Sprite Load(string iconKey);
    }
}