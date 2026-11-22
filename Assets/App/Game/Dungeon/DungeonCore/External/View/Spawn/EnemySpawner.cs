using App.Common.ModuleItem.Runtime.Config;
using App.Game.AI.Runtime;
using UnityEngine;
using Zenject;

namespace App.Game.Dungeon.DungeonCore.External.View.Spawn
{
    [DisallowMultipleComponent]
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private ModuleItemGameConfig _enemy;

        private IAIController _aiController;

        [Inject]
        private void Construct([InjectOptional] IAIController aiController)
        {
            _aiController = aiController;
        }

        private void Start()
        {
            if (_aiController == null)
                return;

            if (_enemy == null)
            {
                Debug.LogError("EnemySpawner requires an enemy config.", this);
                return;
            }

            if (!_aiController.Create(_enemy.Id, transform.position).HasValue)
                Debug.LogError($"EnemySpawner could not spawn '{_enemy.Id}'.", this);
        }
    }
}
