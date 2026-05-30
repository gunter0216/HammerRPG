using App.Common.Logger.Runtime;
using App.Common.ModuleItem.External;
using App.Common.Utilities.UtilityUnity.Runtime;
using App.Game.Modules.Race.Runtime.Config;
using App.Game.Player.External.View;
using UnityEngine;

namespace App.Game.Dungeon.DungeonCore.External.Controllers
{
    public class AITargetDetector : MonoBehaviour
    {
        [SerializeField] private float _radius = 10f;
        [SerializeField] private LayerMask _targetMask = Physics.AllLayers;

        public EntityView FindTarget(EntityView self)
        {
            int count = RayCastHelper.OverlapSphereNonAlloc(
                transform.position,
                _radius,
                out var results,
                _targetMask);

            if (!self.TryGetComponent<ModuleItemView>(out var selfModuleItemView))
            {
                HLogger.LogError("ModuleItemView not found.");
                return null;
            }
            
            if (!selfModuleItemView.ModuleItem.TryGetConfigModule<RaceModuleConfig>(out var raceConfig))
            {
                HLogger.LogError($"RaceModuleConfig not found.");
                return null;
            }
            
            var selfRace = raceConfig.Race;

            EntityView closest = null;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                var collider = results[i];
                if (!collider.TryGetComponent(out EntityView entity))
                {
                    continue;
                }

                if (entity == self)
                {
                    continue;
                }

                if (!entity.TryGetComponent<ModuleItemView>(out var entityModuleItemView))
                {
                    HLogger.LogError("ModuleItemView not found.");
                    return null;
                }
                
                if (!entityModuleItemView.ModuleItem.TryGetConfigModule<RaceModuleConfig>(out raceConfig))
                {
                    continue;   
                }
                
                var entityRace = raceConfig.Race;
                if (entityRace == selfRace)
                {
                    continue;
                }

                float sqrDistance =
                    (entity.transform.position - transform.position).sqrMagnitude;

                if (sqrDistance < closestDistance)
                {
                    closestDistance = sqrDistance;
                    closest = entity;
                }
            }

            return closest;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}