using App.Game.GameManagers.External.View;
using App.Game.GameTiles.Runtime;

namespace App.Game.GameManagers.External
{
    public class GameTile
    {
        private readonly ITile m_Tile;
        private readonly ITileView m_View;

        public ITile Tile => m_Tile;

        public ITileView View => m_View;

        public GameTile(ITile tile, ITileView view)
        {
            m_Tile = tile;
            m_View = view;
        }
    }
}