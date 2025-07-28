using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Utility.Runtime;
using App.Game.Configs.Runtime;
using App.Game.Contexts;
using App.Game.GameTiles.External.Config.Converter;
using App.Game.GameTiles.External.Config.Loader;
using App.Game.GameTiles.External.Config.Service;
using App.Game.GameTiles.Runtime;
using App.Game.GameTiles.Runtime.Data;
using App.Game.SpriteLoaders.Runtime;
using App.Game.States.Game;
using UnityEngine;
using Logger = App.Common.Logger.Runtime.Logger;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Game.GameTiles.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    public class TilesController : IInitSystem, ITilesController
    {
        [Inject] private readonly IConfigLoader m_ConfigLoader;
        [Inject] private readonly ISpriteLoader m_SpriteLoader;

        private TilesConfigService m_ConfigService;
        
        public void Init()
        {
            if (!InitConfig())
            {
                Logger.LogError("Cant inti config service.");
                return;
            }
        }

        private bool InitConfig()
        {
            var loader = new TilesConfigLoader(m_ConfigLoader);
            var converter = new TilesDtoToConfigConverter();
            var dto = loader.Load();
            if (!dto.HasValue)
            {
                return false;
            }

            var config = converter.Convert(dto.Value);
            if (!config.HasValue)
            {
                return false;
            }
            
            m_ConfigService = new TilesConfigService(config.Value);
            m_ConfigService.Initialize();
            
            return true;
        }

        public Optional<ITile> CreateTileByGenerationID(string generationID, Vector2Int position)
        {
            var config = m_ConfigService.GetTileByGenerationId(generationID);
            if (!config.HasValue)
            {
                return Optional<ITile>.Fail();
            }

            var data = new TileData()
            {
                ID = config.Value.ID,
                PositionX = position.X,
                PositionY = position.Y
            };
            var tile = new Tile(config.Value, data);
            return Optional<ITile>.Success(tile);
        }

        public Optional<Sprite> GetTileSprite(ITile tile)
        {
            return m_SpriteLoader.Load(tile.Config.SpriteKey);
        }
    }
}