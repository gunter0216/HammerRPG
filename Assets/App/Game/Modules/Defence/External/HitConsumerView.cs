using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Modules.Defence.Runtime;
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

            foreach (var handler in _handlers)
            {
                handler.Initialize(_moduleItem, OnDamaged);
            }
        }

        private void OnDamaged(SimpleDamageHandler handler, HitModel model)
        {
            if (_moduleItem == null)
            {
                HLogger.LogError("_moduleItem is null");
                return;
            }

            if (!_moduleItem.TryGetModule<DefenceModule>(out var defenceModule))
            {
                HLogger.LogError("defenceModule not found.");
                return;
            }

            defenceModule.OnDamage(model, handler);
        }
    }
}