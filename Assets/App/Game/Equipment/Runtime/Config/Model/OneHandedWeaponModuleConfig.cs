using System;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;

namespace App.Game.Equipment.Runtime.Config.Model
{
    [Serializable]
    public sealed class OneHandedWeaponModuleConfig : ModuleConfig
    {
        [SerializeField] private Sprite _nearSprite;
        [SerializeField] private Sprite _farSprite;
        [SerializeField, Min(0)] private float _damage = 10f;
        [SerializeField, Min(0.01f), Tooltip("Множитель скорости атаки: 1 — обычная, 2 — вдвое быстрее.")]
        private float _attackSpeed = 1f;
        [SerializeField, Min(0.01f)] private float _reach = 1.45f;
        [SerializeField, Min(0.01f)] private float _sweepWidth = 1.3f;

        public Sprite NearSprite => _nearSprite;
        public Sprite FarSprite => _farSprite;
        public float Damage => _damage;
        public float AttackSpeed => _attackSpeed;
        public bool IsAttackSpeedValid => _attackSpeed > 0 && !float.IsInfinity(_attackSpeed);
        public float Reach => _reach;
        public float SweepWidth => _sweepWidth;

        public OneHandedWeaponModuleConfig() { }

        public OneHandedWeaponModuleConfig(Sprite nearSprite, Sprite farSprite)
        {
            _nearSprite = nearSprite;
            _farSprite = farSprite;
        }
    }
}
