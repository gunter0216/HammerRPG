using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Player.Runtime.Components;
using App.Game.Worlds.Runtime;
using App.Generation.DungeonCreator.Runtime;
using UnityEngine;

namespace App.Game.GameManagers.External
{
    public class GameManager : IInitSystem
    {
        private readonly IWorldManager m_WorldManager;
        private readonly IDungeonCreator m_DungeonCreator;

        private Dungeon m_Dungeon;

        public GameManager(IWorldManager worldManager, IDungeonCreator dungeonCreator)
        {
            m_WorldManager = worldManager;
            m_DungeonCreator = dungeonCreator;
        }

        public void Init()
        {
            if (!CreateDungeon())
            {
                HLogger.LogError("Cant generate dungeon");
                return;
            }

            PlacePlayerOnStartRoom();
        }

        private bool CreateDungeon()
        {
            var dungeon = m_DungeonCreator.Create();
            if (!dungeon.HasValue)
            {
                HLogger.LogError("Cant create dungeon.");
                return false;
            }

            m_Dungeon = dungeon.Value;
            
            return true;
        }

        private void PlacePlayerOnStartRoom()
        {
            var startRoom = m_Dungeon.StartRoom;
            var position = startRoom.GetCenter();
            
            var world = m_WorldManager.GetWorld();
            var entityPool = world.GetPool<EntityComponent>();

            foreach (var i in world.Filter<PlayerComponent>().End())
            {
                var entity = entityPool.Get(i);
                entity.View.transform.position = new Vector3(position.X, position.Y);
            }
        }
    }
}