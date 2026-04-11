using App.Common.Utilities.Utility.Runtime;
using App.Common.Utilities.UtilityUnity.Runtime.Extensions;
using App.Game.Player.External;
using App.Game.Player.Runtime.Components;
using UnityEngine;

namespace App.Game.CameraFollow.External
{
    public class CameraFollowController : IInitSystem, IUpdateSystem
    {
        private readonly PlayerController _playerController;
        
        private Camera _camera;

        public CameraFollowController(PlayerController playerController)
        {
            _playerController = playerController;
        }

        public void Init()
        {
            _camera = Camera.main;
        }

        public void OnUpdate()
        {
            var playerPosition = _playerController.PlayerView.Transform.position;

            _camera.transform.SetPositionX(playerPosition.x);
            _camera.transform.SetPositionY(playerPosition.y);
        }
    }
}