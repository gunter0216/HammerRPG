using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Dungeon.DungeonCore.Runtime;
using App.Game.Player.External.View;
using UnityEngine;

namespace App.Game.Player.External
{
    public class PlayerController : IInitSystem
    {
        private readonly IDungeonController _dungeonController;
        private readonly IAssetManager _assetManager;
        
        private EntityView _view;

        public PlayerController(IDungeonController dungeonController, IAssetManager assetManager)
        {
            _dungeonController = dungeonController;
            _assetManager = assetManager;
        }

        public void Init()
        {
            CreateView();
            PlacePlayerOnStartRoom();
        }

        private void CreateView()
        {
            var viewCreator = new PlayerViewCreator(_assetManager);
            var viewResult = viewCreator.Create();
            if (!viewResult.HasValue)
            {
                HLogger.LogError("Cant create view.");
                return;
            }

            _view = viewResult.Value;
        }

        private void PlacePlayerOnStartRoom()
        {
            var position = _dungeonController.GetSpawnPoint();
            if (!position.HasValue)
            {
                return;
            }
            
            _view.transform.position = new Vector3(position.Value.X, position.Value.Y);
        }
    }
}