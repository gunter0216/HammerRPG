using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.AI.Runtime;
using App.Game.Dungeon.DungeonCore.External.Controllers;
using Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Model;
using UnityEngine;

namespace App.Game.Dungeon.DungeonCore.External.View.Spawn
{
    public class EnemySpawnController
    {
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly IAssetManager _assetManager;
        private readonly IAIController _aiController;

        private readonly float _minDistanceBetweenEnemies = 2f;
        private readonly int _maxGlobalAttempts = 300;

        private readonly List<ISpawnArea> _areas = new();
        private Transform _roomView;

        private List<AIViewController> _aiControllers;

        public EnemySpawnController(
            IModuleItemsManager moduleItemsManager, 
            IAssetManager assetManager,
            IAIController aiController)
        {
            _moduleItemsManager = moduleItemsManager;
            _assetManager = assetManager;
            _aiController = aiController;
        }

        public void Initialize(Transform view)
        {
            _roomView = view;

            var behaviours = _roomView.GetComponentsInChildren<MonoBehaviour>();

            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour is ISpawnArea area)
                    _areas.Add(area);
            }
        }

        public void SpawnEnemies()
        {
            var spawnPoints = GetSpawnPositions(1);
            _aiControllers = new List<AIViewController>(3);
            foreach (var spawnPosition in spawnPoints)
            {
                var controller = _aiController.Create("Goblin", spawnPosition);
                _aiControllers.Add(controller.Value);
            }
        }

        public List<Vector3> GetSpawnPositions(int count)
        {
            List<Vector3> positions = new();

            if (_areas.Count == 0)
            {
                Debug.LogWarning("No spawn areas found");
                return positions;
            }

            int attempts = 0;

            while (positions.Count < count &&
                   attempts < _maxGlobalAttempts)
            {
                attempts++;

                ISpawnArea area = GetWeightedRandomArea();

                if (!area.TryGetSpawnPoint(out Vector3 point))
                    continue;

                if (IsFarEnough(point, positions))
                {
                    positions.Add(point);
                }
            }

            if (positions.Count < count)
            {
                Debug.LogWarning(
                    $"Requested {count} spawn points, " +
                    $"but only found {positions.Count}");
            }

            return positions;
        }

        private ISpawnArea GetWeightedRandomArea()
        {
            float totalWeight = 0f;

            foreach (ISpawnArea area in _areas)
            {
                totalWeight += area.Weight;
            }

            float random =
                Random.Range(0f, totalWeight);

            float current = 0f;

            foreach (ISpawnArea area in _areas)
            {
                current += area.Weight;

                if (random <= current)
                    return area;
            }

            return _areas[^1];
        }

        private bool IsFarEnough(
            Vector3 point,
            List<Vector3> existing)
        {
            float sqrMinDistance =
                _minDistanceBetweenEnemies *
                _minDistanceBetweenEnemies;

            foreach (Vector3 other in existing)
            {
                if ((point - other).sqrMagnitude <
                    sqrMinDistance)
                {
                    return false;
                }
            }

            return true;
        }
    }
}