using System;
using App.Common.ModuleItem.Runtime;
using UnityEngine;

namespace Game.Project.Gameplay.Weapon.Runtime.DamageHandlers
{
    [DisallowMultipleComponent]
    public class SimpleDamageHandler : MonoBehaviour, IDamageHandler
    {
        private Action<SimpleDamageHandler, HitModel> _callback;

        public void Initialize(IModuleItem moduleItem, Action<SimpleDamageHandler, HitModel> callback)
        {
            ModuleItem = moduleItem;
            _callback = callback;
        }


        public IModuleItem ModuleItem { get; private set; }

        public void Handle(HitModel model)
        {
            _callback?.Invoke(this, model);
        }
    }
}