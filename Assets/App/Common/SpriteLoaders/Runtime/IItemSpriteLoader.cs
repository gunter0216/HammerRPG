using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Common.SpriteLoaders.External
{
    public interface IItemSpriteLoader : ISpriteLoader
    {
        Optional<Sprite> LoadItemSprite(string itemId);
        Optional<Sprite> LoadItemSprite(IModuleItem item);
    }
}