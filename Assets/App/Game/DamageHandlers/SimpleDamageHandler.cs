using System;
using UnityEngine;

namespace Game.Project.Gameplay.Weapon.Runtime.DamageHandlers
{
    [DisallowMultipleComponent]
    public class SimpleDamageHandler : MonoBehaviour, IDamageHandler
    {
        private Action<SimpleDamageHandler, HitModel> _callback;

        public void Initialize(string id, Action<SimpleDamageHandler, HitModel> callback)
        {
            Id = id;
            _callback = callback;
        }

        public string Id { get; private set; }

        public void Handle(HitModel model)
        {
            _callback?.Invoke(this, model);
        }
    }
}