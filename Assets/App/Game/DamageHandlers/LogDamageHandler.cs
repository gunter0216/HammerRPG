using UnityEngine;

namespace Game.Project.Gameplay.Weapon.Runtime.DamageHandlers
{
    public class LogDamageHandler : MonoBehaviour, IDamageHandler
    {
        public string Id => string.Empty;

        public void Handle(HitModel model)
        {
            Debug.LogError($"Hit {gameObject.name} {model.Damage}");
        }
    }
}