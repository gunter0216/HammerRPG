using App.Common.AssetSystem.External;
using App.Common.AssetSystem.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Dungeon.DungeonCore.External.Controllers;
using App.Game.StatusBar.Runtime;
using Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Model;
using UnityEngine;
using UnityEngine.AI;

namespace App.Game.AI.External.Fabric
{
    public class AICreator
    {
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly IAssetManager _assetManager;
        private readonly IStatusBarController _statusBarController;
        private readonly Transform _root;

        public AICreator(IAssetManager assetManager, IModuleItemsManager moduleItemsManager, IStatusBarController statusBarController)
        {
            _assetManager = assetManager;
            _moduleItemsManager = moduleItemsManager;
            _statusBarController = statusBarController;
            
            _root = new GameObject("AI").transform;
        }

        public Optional<AIViewController> Create(string id, Vector3 position)
        {
            var enemy = _moduleItemsManager.Create(id);
            var module = enemy.Value.GetConfigModule<AssetModuleConfig>();
            var assetKey = module.Value.AssetKey;

            var viewResult = _assetManager.InstantiateSync<Transform>(assetKey);
            var view = viewResult.Value;
            var agent = view.GetComponent<NavMeshAgent>();
            
            view.SetParent(_root);
            agent.Warp(position);
            
            var aiController = new AIViewController(_statusBarController, enemy.Value, view);
            aiController.Activate();
            
            return Optional<AIViewController>.Success(aiController);
        }
    }
}