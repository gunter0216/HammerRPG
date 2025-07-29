using App.Game.GameTiles.External.Config.Model;
using App.Game.GameTiles.Runtime.Data;

namespace App.Game.GameTiles.Runtime
{
    public class Tile : ITile
    {
        private readonly TileData m_Data;
        private readonly TileConfig m_Config;

        public TileData Data => m_Data;
        public TileConfig Config => m_Config;

        public Tile(TileConfig config, TileData data)
        {
            m_Config = config;
            m_Data = data;
        }
    }
}