using System;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;

namespace App.Game.Equipment.Runtime.Config.Model
{
    [Serializable]
    public sealed class ShieldModuleConfig : ModuleConfig
    {
        [SerializeField] private Sprite _nearSprite;
        [SerializeField] private Sprite _farSprite;

        public Sprite NearSprite => _nearSprite;
        public Sprite FarSprite => _farSprite;

        public ShieldModuleConfig() { }

        public ShieldModuleConfig(Sprite nearSprite, Sprite farSprite)
        {
            _nearSprite = nearSprite;
            _farSprite = farSprite;
        }
    }
}
