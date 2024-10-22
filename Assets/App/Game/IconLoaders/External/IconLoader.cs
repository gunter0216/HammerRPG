using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.HammerDI.Runtime.Attributes;
using App.Common.Utility.Runtime.Extensions;
using App.Game.IconLoaders.Runtime;
using App.Game.States.Start;
using UnityEngine;

namespace App.Game.IconLoaders.External
{
    [Singleton]
    [Stage(typeof(StartInitPhase), -10)]
    public class IconLoader : IInitSystem, IIconLoader
    {
        private const string m_TransparentImageKey = "TransparentImage";
        
        [Inject] private IAssetManager m_AssetManager;

        private readonly HashSet<string> m_LoadedIcons = new();

        private Sprite m_TransparentImage;
        
        public void Init()
        {
            m_TransparentImage = m_AssetManager.LoadSync<Sprite>(new StringKeyEvaluator(m_TransparentImageKey)).Value;
        }

        public Sprite Load(string iconKey)
        {
            if (iconKey.IsNullOrEmpty())
            {
                Debug.LogError($"[IconController] In method LoadSprite, iconKey is null or empty.");
                return m_TransparentImage;
            }
            
            var sprite = m_AssetManager.LoadSync<Sprite>(new StringKeyEvaluator(iconKey));
            if (!sprite.HasValue)
            {
                Debug.LogError(
                    $"[IconController] In method LoadSprite, error load sprite {iconKey}.");
                return m_TransparentImage;
            }
            
            m_LoadedIcons.Add(iconKey);
            
            return sprite.Value;
        }

        public Sprite GetTransparentImage()
        {
            return m_TransparentImage;
        }

        public void UnloadContextIcons()
        {
            foreach (var loadedIcon in m_LoadedIcons)
            {
                m_AssetManager.UnloadAsset(new StringKeyEvaluator(loadedIcon));
            }
        }
    }
}