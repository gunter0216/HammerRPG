using App.Common.Autumn.Runtime.Attributes;
using App.Common.Configs.Runtime;
using App.Common.DataContainer.Runtime;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.External;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Contexts;
using App.Game.GameTiles.External.Config.Data;
using App.Game.GameTiles.External.Config.Loader;
using App.Game.GameTiles.External.Config.Model;
using App.Game.GameTiles.Runtime;
using App.Game.SpriteLoaders.Runtime;
using App.Game.States.Runtime.Game;
using UnityEngine;
using Vector2Int = App.Common.Algorithms.Runtime.Vector2Int;

namespace App.Game.GameTiles.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    public class TilesController : IInitSystem, ITilesController
    {
        [Inject] private readonly IConfigLoader m_ConfigLoader;
        [Inject] private readonly ISpriteLoader m_SpriteLoader;
        [Inject] private readonly ModuleItemsManager m_ModuleItemsManager;

        public void Init()
        {
            if (!InitConfig())
            {
                HLogger.LogError("Cant inti config service.");
                return;
            }

            var configLoader = new TileModuleItemsConfigLoader(m_ConfigLoader);
            m_ModuleItemsManager.RegisterItems(configLoader, TileConstants.ModuleItemType);
        }

        public Optional<ITileModuleItem> Create(DataReference dataReference)
        {
            var item = m_ModuleItemsManager.Create(dataReference);
            if (!item.HasValue)
            {
                return Optional<ITileModuleItem>.Fail();
            }
            
            return Optional<ITileModuleItem>.Success(new TileModuleItem(item.Value));
        }

        public Optional<ITileModuleItem> Create(string id)
        {
            var item = m_ModuleItemsManager.Create(id);
            if (!item.HasValue)
            {
                return Optional<ITileModuleItem>.Fail();
            }
            
            return Optional<ITileModuleItem>.Success(new TileModuleItem(item.Value));
        }

        public bool Destroy(ITileModuleItem data)
        {
            return m_ModuleItemsManager.Destroy(data);
        }

        private bool InitConfig()
        {
            // var loader = new TilesConfigLoader(m_ConfigLoader);
            // var converter = new TilesDtoToConfigConverter();
            // var dto = loader.Load();
            // if (!dto.HasValue)
            // {
            //     return false;
            // }
            //
            // var config = converter.Convert(dto.Value);
            // if (!config.HasValue)
            // {
            //     return false;
            // }
            //
            // m_ConfigService = new TilesConfigService(config.Value);
            // m_ConfigService.Initialize();
            
            return true;
        }

        public Optional<ITileModuleItem> CreateTileByGenerationID(string generationID, Vector2Int position)
        {
            var tileID = GenerationIdToTileConvert(generationID);
            var tile = Create(tileID);
            if (!tile.HasValue)
            {
                return Optional<ITileModuleItem>.Fail();
            }

            tile.Value.AddDataModule(new TilePositionModuleData(position.X, position.Y));
            
            return tile;
        }

        public Optional<Sprite> GetTileSprite(string tileID)
        {
            var config = m_ModuleItemsManager.GetConfig(tileID);
            if (!config.HasValue)
            {
                return Optional<Sprite>.Fail();
            }

            var tileSprite = config.Value.GetModule<SpriteModuleConfig>();
            if (!tileSprite.HasValue)
            {
                return Optional<Sprite>.Fail();
            }
            
            return m_SpriteLoader.Load(tileSprite.Value.Key);
        }
        
        public Optional<Sprite> GetTileSprite(ITileModuleItem tileModuleItem)
        {
            return GetTileSprite(tileModuleItem.Id);
        }

        private string GenerationIdToTileConvert(string generationId)
        {
            if (generationId == "Wall")
            {
                return "wall";
            }
            
            if (generationId == "Door")
            {
                return "closed_door";
            }
            
            if (generationId == "OpenedDoor")
            {
                return "opened_door";
            }

            return null;
        }
    }
}