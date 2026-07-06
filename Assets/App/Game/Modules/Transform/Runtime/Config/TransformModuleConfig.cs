using App.Common.ModuleItem.Runtime.Config.Interfaces;
using UnityEngine;

namespace App.Game.Modules.Transform.Runtime.Config
{
    public class TransformModuleConfig : ModuleConfig
    {
        [SerializeField] private Vector3 _position;
        
        public TransformModuleConfig()
        {
        }
    }
}