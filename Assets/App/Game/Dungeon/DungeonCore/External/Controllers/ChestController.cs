using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.SpriteLoaders.External;
using App.Game.Containers.ContainerWindow.Runtime;
using App.Game.Dungeon.DungeonCore.External.View;
using App.Game.Dungeon.DungeonCore.Runtime.Services;
using App.Game.Dungeon.DungeonCreator.Runtime.Chest;
using App.Game.FollowIcon.External;
using App.Game.Modules.Chests.Runtime;
using Assets.App.Game.Interactions.Runtime;
using Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Model;
using UnityEngine;

namespace App.Game.Dungeon.DungeonCore.External.Controllers
{
    public class ChestController
    {
        private static readonly int _opened = Animator.StringToHash("Opened");
        
        private const string _iconChest = "FollowIcon_Chest";
        private const string _iconEmptyChest = "FollowIcon_EmptyChest";

        private readonly IFollowIconController _followIconController;
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly ChestInteractiveView _view;
        private readonly RoomService _roomService;

        private Animator _animator;
        private ChestService _chestService;
        private ChestModule _chest;

        public ChestController(IContainerWindowController containerWindow,
            IFollowIconController followIconController,
            IModuleItemsManager moduleItemsManager,
            ChestInteractiveView view,
            DungeonService dungeonService, 
            RoomService roomService)
        {
            _followIconController = followIconController;
            _moduleItemsManager = moduleItemsManager;
            _view = view;
            _roomService = roomService;
        }

        public void Initialize()
        {
            _animator = _view.GetComponent<Animator>();
            
            var interactableView = _view.gameObject.AddComponent<InteractableView>();
            interactableView.OnClickCallback += OnButtonClick;
            interactableView.OnHoverEnterCallback += OnButtonEnter;
            interactableView.OnHoverExitCallback += OnButtonExit;
            
            _chestService = _roomService.CreateChest();
            _chest = _chestService.Module;
        }

        private void OnButtonEnter()
        {
            // var icon = _chest.ChestModule.IsUsed ? _iconEmptyChest : _iconChest;
            // _followIconController.Show(this, icon);
            
            // _animator.SetBool(_opened, true);
        }

        private void OnButtonExit()
        {
            // _followIconController.Hide(this);
            
            // _animator.SetBool(_opened, false);
        }

        private void OnButtonClick()
        {
            if (_chest.IsClosed)
            {
                HLogger.LogError("Chest is closed.");
                return;
            }

            _chestService.Open(OnClosedCallback);
            
            _animator.SetBool(_opened, true);
        }

        private void OnClosedCallback()
        {
            _animator.SetBool(_opened, false);
        }
    }
}