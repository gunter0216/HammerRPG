using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Modules.Health.Runtime;
using App.Game.Modules.Health.Runtime.Data;
using UnityEngine;

namespace Game.Project.Gameplay.Weapon.Runtime.DamageHandlers
{
    public class HitConsumerView : MonoBehaviour
    {
        [SerializeField] private SimpleDamageHandler[] _handlers;
        
        private IModuleItem _moduleItem;

        public void SetModuleItem(IModuleItem moduleItem)
        {
            _moduleItem = moduleItem;

            var id = _moduleItem.Id;
            foreach (var handler in _handlers)
            {
                handler.Initialize(id, OnDamaged);
            }
        }

        private void OnDamaged(SimpleDamageHandler handler, HitModel model)
        {
            if (_moduleItem == null)
            {
                HLogger.LogError("_moduleItem is null");
                return;
            }

            var healthData = _moduleItem.GetModule<HealthModule>();
            healthData.Value.Spend(10);
            HLogger.LogError($"value {healthData.Value}");
        }
    }
}