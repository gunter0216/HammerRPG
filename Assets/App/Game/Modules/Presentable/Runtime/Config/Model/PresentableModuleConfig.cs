using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;

namespace App.Game.Modules.Asset.Runtime.Config.Model
{
    public class PresentableModuleConfig : ModuleConfig
    {
        [SerializeField] private GameObject _asset;

        public GameObject Asset => _asset;

        public PresentableModuleConfig(GameObject asset)
        {
            _asset = asset;
        }

        public GameObject Instantiate(Transform parent = null)
        {
            return Object.Instantiate(_asset, parent);
        }
    }
}