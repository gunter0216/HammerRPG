using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Utility.Runtime;
using App.Game.GameManagers.External.View;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.Game.GameManagers.External.Services
{
    public class TileViewCreator : IDisposable
    {
        private const string m_TileViewAssetKey = "TileView";
        
        private readonly IAssetManager m_AssetManager;
        
        private TileView m_ViewPrefab;

        public TileViewCreator(IAssetManager assetManager)
        {
            m_AssetManager = assetManager;
        }

        public bool Initialize()
        {
            var prefab = m_AssetManager.LoadSync<GameObject>(new StringKeyEvaluator(m_TileViewAssetKey));
            if (!prefab.HasValue) 
            {
                return false;
            }

            m_ViewPrefab = prefab.Value.GetComponent<TileView>();
            return true;
        }

        public Optional<ITileView> Create()
        {
            var view = Object.Instantiate(m_ViewPrefab);
            if (view == null)
            {
                return Optional<ITileView>.Fail();
            }
            
            return Optional<ITileView>.Success(view);
        }

        public void Dispose()
        {
            m_AssetManager.UnloadAsset(new StringKeyEvaluator(m_TileViewAssetKey));
        }
    }
}