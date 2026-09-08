using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;

namespace App.Game.Modules.Move.Runtime.Config
{
    public class MoveModuleConfig : ModuleConfig
    {
        [SerializeField] private float _speed;

        public float Speed => _speed;

        public MoveModuleConfig(float speed)
        {
            _speed = speed;
        }
    }
}