using System.Collections.Generic;
using App.Common.AssetSystem.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.AI.External.Fabric;
using App.Game.AI.Runtime;
using App.Game.Dungeon.DungeonCore.External.Controllers;
using App.Game.Player.External;
using App.Game.StatusBar.Runtime;
using Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Model;
using UnityEngine;
using UnityEngine.AI;

namespace App.Game.AI.External
{
    public class AIController : IInitSystem, IAIController, IUpdateSystem
    {
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly IAssetManager _assetManager;
        private readonly IStatusBarController _statusBarController;
        
        private AICreator _aiCreator;

        private List<AIViewController> _controllers;

        public AIController(IModuleItemsManager moduleItemsManager, IAssetManager assetManager, IStatusBarController statusBarController)
        {
            _moduleItemsManager = moduleItemsManager;
            _assetManager = assetManager;
            _statusBarController = statusBarController;
        }

        public void Init()
        {
            _controllers = new List<AIViewController>();

            _aiCreator = new AICreator(_assetManager, _moduleItemsManager, _statusBarController);
        }

        public Optional<AIViewController> Create(string id, Vector3 position)
        {
            var aiController = _aiCreator.Create(id, position);
            if (aiController.HasValue)
            {
                _controllers.Add(aiController.Value);
            }
            
            return aiController;
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