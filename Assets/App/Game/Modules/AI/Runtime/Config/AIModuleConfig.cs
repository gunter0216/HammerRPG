using System;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;

namespace App.Game.Modules.AI.Runtime.Config
{
    [Serializable]
    public sealed class AIModuleConfig : ModuleConfig
    {
        [SerializeField, Min(0.01f), Tooltip("Радиус обнаружения цели в мировых единицах.")]
        private float _detectionRadius = 5f;

        [SerializeField, Min(0.01f), Tooltip("Интервал поиска новой цели в секундах.")]
        private float _targetScanInterval = 0.2f;

        public float DetectionRadius => _detectionRadius;
        public float TargetScanInterval => _targetScanInterval;

        public bool IsValid =>
            _detectionRadius > 0 && !float.IsInfinity(_detectionRadius) &&
            _targetScanInterval > 0 && !float.IsInfinity(_targetScanInterval);
    }
}
