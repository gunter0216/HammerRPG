using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Dungeon.DungeonCore.External.Controllers;
using App.Game.Modules.Asset.Runtime.Config.Model;
using App.Game.Modules.Move.Runtime;
using App.Game.StatusBar.Runtime;
using UnityEngine;
using UnityEngine.AI;

namespace App.Game.AI.External.Fabric
{
    public class AICreator
    {
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly MoveModuleSystem _moveModules;
        private readonly IStatusBarController _statusBarController;
        private readonly Transform _root;

        public AICreator(IModuleItemsManager moduleItemsManager, MoveModuleSystem moveModules,
            IStatusBarController statusBarController)
        {
            _moduleItemsManager = moduleItemsManager;
            _moveModules = moveModules;
            _statusBarController = statusBarController;
            
            _root = new GameObject("AI").transform;
        }

        public Optional<AIViewController> Create(string id, Vector3 position)
        {
            var enemy = _moduleItemsManager.Create(id);
            if (!enemy.HasValue)
                return Optional<AIViewController>.Fail();

            var item = enemy.Value;
            if (!item.TryGetConfigModule<PresentableModuleConfig>(out var presentable) || presentable.Asset == null)
                return FailSpawn(item);

            var view = presentable.Instantiate(_root).transform;
            if (view.TryGetComponent<Rigidbody2D>(out _))
                view.position = new Vector3(position.x, position.y, 0);
            else if (!view.TryGetComponent<NavMeshAgent>(out var agent) || !agent.Warp(position))
                return FailSpawn(item, view);

            var aiController = new AIViewController(_statusBarController, _moveModules, item, view);
            return aiController.Activate()
                ? Optional<AIViewController>.Success(aiController)
                : FailSpawn(item, view);
        }

        private Optional<AIViewController> FailSpawn(IModuleItem item, Transform view = null)
        {
            if (view != null)
                UnityEngine.Object.Destroy(view.gameObject);
            if (!_moduleItemsManager.Destroy(item))
                Debug.LogError($"Could not clean up enemy '{item.Id}' after a failed spawn.");
            return Optional<AIViewController>.Fail();
        }
    }
}
