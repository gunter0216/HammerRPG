using App.Common.Autumn.Runtime.Attributes;
using App.Common.FSM.Runtime;
using App.Common.FSM.Runtime.Attributes;
using App.Common.ModuleItem.Runtime;
using App.Game.Contexts;
using App.Game.GameItems.External;
using App.Game.States.Game;

namespace App.Game.Cheats.External
{
    [Scoped(typeof(GameSceneContext))]
    [Stage(typeof(GameInitPhase), 0)]
    public class CheatsController : IInitSystem
    {
        [Inject] private readonly IModuleItemsManager m_ModuleItemsManager;
        
        public void Init()
        {
            var configs = m_ModuleItemsManager.GetConfigs(GameItemsConstants.ModuleItemType);
            
        }
    }
}