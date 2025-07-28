using System.Collections.Generic;
using App.Common.Utility.Runtime;
using App.Game.GameTiles.External.Config.Model;

namespace App.Game.GameTiles.External.Config.Service
{
    public class TilesConfigService
    {
        private readonly TilesConfig m_Config;

        private Dictionary<string, TileConfig> m_GenerationIdToTile;

        public TilesConfigService(TilesConfig config)
        {
            m_Config = config;
        }

        public void Initialize()
        {
            m_GenerationIdToTile = new Dictionary<string, TileConfig>(m_Config.Tiles.Count);
            foreach (var tileConfig in m_Config.Tiles)
            {
                m_GenerationIdToTile.Add(tileConfig.GenerationID, tileConfig);
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
    }
}