using System.Collections.Generic;
using App.Game.GameManagers.External;

namespace App.Game.GameTiles.External.Config.Model
{
    public class TilesConfig
    {
        private readonly IReadOnlyList<TileConfig> m_Tiles;

        public IReadOnlyList<TileConfig> Tiles => m_Tiles;

        public TilesConfig(IReadOnlyList<TileConfig> tiles)
        {
            m_Tiles = tiles;
        }
    }
}