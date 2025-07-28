using System.Collections.Generic;
using App.Common.Utility.Runtime;
using App.Game.GameTiles.External.Config.Model;

namespace App.Game.GameTiles.External.Config.Service
{
    public class TilesConfigService
    {
        private readonly TilesConfig m_Config;

        private Dictionary<string, TileConfig> m_GenerationIdToTile;
        private Dictionary<string, TileConfig> m_IdToTile;

        public TilesConfigService(TilesConfig config)
        {
            m_Config = config;
        }

        public void Initialize()
        {
            m_GenerationIdToTile = new Dictionary<string, TileConfig>(m_Config.Tiles.Count);
            m_IdToTile = new Dictionary<string, TileConfig>(m_Config.Tiles.Count);
            foreach (var tileConfig in m_Config.Tiles)
            {
                m_GenerationIdToTile.Add(tileConfig.GenerationID, tileConfig);
                m_IdToTile.Add(tileConfig.ID, tileConfig);
            }
        }

        public Optional<TileConfig> GetTileByGenerationId(string generationId)
        {
            if (m_GenerationIdToTile.TryGetValue(generationId, out var tileConfig))
            {
                return Optional<TileConfig>.Success(tileConfig);
            }
            
            return Optional<TileConfig>.Fail();
        }

        public Optional<TileConfig> GetTile(string tileID)
        {
            if (m_IdToTile.TryGetValue(tileID, out var tileConfig))
            {
                return Optional<TileConfig>.Success(tileConfig);
            }
            
            return Optional<TileConfig>.Fail();
        }
    }
}