using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Game.Configs.Runtime;
using App.Game.Contexts;
using App.Game.GameTiles.External.Config.Converter;
using App.Game.GameTiles.External.Config.Loader;
using App.Game.GameTiles.External.Config.Service;
using App.Game.States.Game;

namespace App.Game.GameTiles.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    public class TilesController : IInitSystem
    {
        [Inject] private readonly IConfigLoader m_ConfigLoader;

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
            
            return true;
        }
    }
}