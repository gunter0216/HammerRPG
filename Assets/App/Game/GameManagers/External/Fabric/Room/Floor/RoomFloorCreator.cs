using App.Common.Logger.Runtime;
using App.Game.GameManagers.External.Room;
using App.Game.GameTiles.External;
using UnityEngine;

namespace App.Game.GameManagers.External.Fabric.Room.Floor
{
    public class RoomFloorCreator
    {
        private readonly TilesController m_TilesController;

        public RoomFloorCreator(TilesController tilesController)
        {
            m_TilesController = tilesController;
        }

        public void CreateFloor(GameRoom room)
        {
            var sprite = m_TilesController.GetTileSprite("floor");
            if (!sprite.HasValue)
            {
                HLogger.LogError("Cant get tile sprite");
                return;
            }

            var floor = new GameObject();
            var position = room.GenerationRoom.GetCenter();
            floor.transform.position = new Vector3(position.X, position.Y, 1);
            floor.transform.parent = room.View.Content;
            var spriteRenderer = floor.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite.Value;
            spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            spriteRenderer.size = new UnityEngine.Vector2(
                room.GenerationRoom.Width, 
                room.GenerationRoom.Height);
        }
    }
}