using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.DungeonCore.External;
using App.Game.Player.Runtime.Components;
using App.Game.Worlds.Runtime;
using App.Generation.DungeonCreator.Runtime;
using UnityEngine;

namespace App.Game.GameManagers.External
{
    public class GameManager : IInitSystem
    {
        private readonly IWorldManager m_WorldManager;
        private readonly IDungeonController _dungeonController;

        public GameManager(IWorldManager worldManager, IDungeonController dungeonController)
        {
            m_WorldManager = worldManager;
            _dungeonController = dungeonController;
        }

        public void Init()
        {
            PlacePlayerOnStartRoom();
        }

        private void PlacePlayerOnStartRoom()
        {
            var position = _dungeonController.GetSpawnPoint();
            if (!position.HasValue)
            {
                return;
            }
            
            var world = m_WorldManager.GetWorld();
            var entityPool = world.GetPool<EntityComponent>();

            foreach (var i in world.Filter<PlayerComponent>().End())
            {
                var entity = entityPool.Get(i);
                entity.View.transform.position = new Vector3(position.Value.X, position.Value.Y);
            }
        }
    }
}