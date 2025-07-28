using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.Logger.Runtime;
using App.Game.Configs.Runtime;
using App.Game.Contexts;
using App.Game.GameManagers.External.Config.Converter;
using App.Game.GameManagers.External.Config.Loader;
using App.Game.GameManagers.External.Config.Service;
using App.Game.States.Game;

namespace App.Game.GameManagers.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    public class GameManager : IInitSystem
    {
        [Inject] private readonly IConfigLoader m_ConfigLoader;

        private GenerationConfigService m_ConfigService;
        
        public void Init()
        {
            if (!InitConfig())
            {
                HLogger.LogError("Cant inti config service.");
                return;
            }

            
        }

        private bool InitConfig()
        {
            var loader = new GenerationConfigLoader(m_ConfigLoader);
            var converter = new GenerationDtoToConfigConverter();
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
            
            m_ConfigService = new GenerationConfigService(config.Value);
            
            return true;
        }
    }
}