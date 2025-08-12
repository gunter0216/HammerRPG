using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.GameManagers.External.Fabric.Tile.View;
using App.Game.GameManagers.External.Room;
using App.Game.GameManagers.External.View;
using App.Game.GameTiles.External;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.DungeonModel;
using UnityEngine;
using Vector2Int = Assets.App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Game.GameManagers.External.Fabric.Tile
{
    public class GameTileCreator
    {
        private readonly TilesController m_TilesController;
        private readonly TileViewCreator m_TileViewCreator;

        public GameTileCreator(TilesController tilesController, TileViewCreator tileViewCreator)
        {
            m_TilesController = tilesController;
            m_TileViewCreator = tileViewCreator;
        }

        public Optional<GameTile> CreateTile(GameRoom room, GeneraitonTile generationTile, Vector2Int localPosition)
        {
            var worldPosition = room.GenerationRoom.LocalToWorld(localPosition);
            
            if (generationTile.Id == TileConstants.Empty)
            {
                return Optional<GameTile>.Fail();
            }

            var isDoor = generationTile.Id == TileConstants.Door;
            var hasLockedDoor = room.GenerationRoom.RequiredKey != null;

            var tile = m_TilesController.CreateTileByGenerationID(generationTile.Id, worldPosition);
            if (!tile.IsSuccess)
            {
                HLogger.LogError($"Cant create tile {tile.ErrorMessage}");
                return Optional<GameTile>.Fail();
            }

            var sprite = !isDoor || hasLockedDoor ? m_TilesController.GetTileSprite(tile.ModuleItem)
                : m_TilesController.GetTileSprite("opened_door");
            if (!sprite.HasValue)
            {
                HLogger.LogError("Cant get tile sprite");
                return Optional<GameTile>.Fail();
            }
            
            var view = CreateTileView();
            if (!view.HasValue)
            {
                HLogger.LogError("Cant create tile view");
                return Optional<GameTile>.Fail();
            }

            view.Value.SetParent(room.View.Content);
            view.Value.SetSprite(sprite.Value);
            view.Value.SetPosition(new Vector3(worldPosition.X + 0.5f, worldPosition.Y + 0.5f));
            if (isDoor && !hasLockedDoor)
            {
                view.Value.DisableCollider();
            }

            var gameTile = new GameTile(tile.ModuleItem, view.Value);
            return Optional<GameTile>.Success(gameTile);
        }
        
        private Optional<ITileView> CreateTileView()
        {
            return m_TileViewCreator.Create();
        }
    }
}