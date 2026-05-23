using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.AI.Runtime;
using App.Game.Dungeon.DungeonCore.External.Controllers;
using App.Game.Player.External;
using Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Model;
using UnityEngine;
using UnityEngine.AI;

namespace App.Game.AI.External
{
    public class AIController : IInitSystem, IAIController, IUpdateSystem
    {
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly IAssetManager _assetManager;
        
        private Transform _root;

        private List<AIViewController> _controllers;

        public AIController(IModuleItemsManager moduleItemsManager, IAssetManager assetManager)
        {
            _moduleItemsManager = moduleItemsManager;
            _assetManager = assetManager;
        }

        public void Init()
        {
            _root = new GameObject("AI").transform;
            _controllers = new List<AIViewController>();
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
            
            var aiController = new AIViewController(enemy.Value, view);
            aiController.Activate();
            
            _controllers.Add(aiController);
            
            return Optional<AIViewController>.Success(aiController);
        }

        public void OnUpdate()
        {
            if (_controllers == null || _controllers.Count <= 0)
            {
                return;
            }
            
            foreach (var controller in _controllers)
            {
                if (!controller.IsActive)
                {
                    continue;
                }

                controller.Update();
            }
        }
    }
}