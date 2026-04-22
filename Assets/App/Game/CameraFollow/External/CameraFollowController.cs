using App.Common.Utilities.Utility.Runtime;
using App.Common.Utilities.UtilityUnity.Runtime.Extensions;
using App.Game.Player.External;
using App.Game.Player.Runtime.Components;
using UnityEngine;

namespace App.Game.CameraFollow.External
{
    public class CameraFollowController : IInitSystem, IUpdateSystem
    {
        private readonly CameraController _cameraController;
        private readonly PlayerController _playerController;
        
        private GameObject _camera;

        public CameraFollowController(PlayerController playerController, CameraController cameraController)
        {
            _playerController = playerController;
            _cameraController = cameraController;
        }

        public void Init()
        {
            _camera = _cameraController.Camera;
        }

        public void OnUpdate()
        {
            var playerPosition = _playerController.PlayerView.Transform.position;

            _camera.transform.SetPositionX(playerPosition.x);
            _camera.transform.SetPositionZ(playerPosition.z);
        }
    }
}