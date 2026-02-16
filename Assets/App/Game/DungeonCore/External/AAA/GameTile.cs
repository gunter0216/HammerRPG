using App.Game.GameManagers.External.View;
using App.Game.GameTiles.Runtime;

namespace App.Game.GameManagers.External
{
    public class GameTile
    {
        private readonly ITileModuleItem m_TileModuleItem;
        private readonly ITileView m_View;

        public ITileModuleItem TileModuleItem => m_TileModuleItem;

        public ITileView View => m_View;

        public GameTile(ITileModuleItem tileModuleItem, ITileView view)
        {
            m_TileModuleItem = tileModuleItem;
            m_View = view;
        }
    }
}