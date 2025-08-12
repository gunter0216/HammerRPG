using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Utilities.Utility.Runtime;
using App.Common.Utilities.Utility.Runtime.Extensions;
using App.Game.SpriteLoaders.Runtime;
using App.Game.States.Runtime.Start;
using UnityEngine;

namespace App.Game.SpriteLoaders.External
{
    [Singleton]
    [Stage(typeof(StartInitPhase), -10)]
    public class SpriteLoader : IInitSystem, ISpriteLoader
    {
        private const string m_TransparentImageKey = "TransparentImage";
        
        [Inject] private IAssetManager m_AssetManager;

        private readonly HashSet<string> m_LoadedIcons = new();

        private Sprite m_TransparentImage;
        
        public void Init()
        {
            m_TransparentImage = m_AssetManager.LoadSync<Sprite>(new StringKeyEvaluator(m_TransparentImageKey)).Value;
        }

        public Optional<Sprite> Load(string key)
        {
            if (key.IsNullOrEmpty())
            {
                Debug.LogError($"[SpriteLoader] In method Load, key is null or empty.");
                return Optional<Sprite>.Fail();
            }
            
            var sprite = m_AssetManager.LoadSync<Sprite>(new StringKeyEvaluator(key));
            if (!sprite.HasValue)
            {
                Debug.LogError($"[SpriteLoader] In method Load, error load sprite {key}.");
                return Optional<Sprite>.Fail();
            }
            
            m_LoadedIcons.Add(key);
            
            return Optional<Sprite>.Success(sprite.Value);
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