// using App.Common.FSM.External;
// using App.Core.Startups.External;
// using App.Core.Startups.External.Attributes;
// using App.Core.Startups.External.Constants;
// using App.Game.Update.External;
//
// namespace App.Game.Worlds.External
// {
//     [Configurator(DIContext.CoreContext)]
//     public class WorldConfigurator : Configurator
//     {
//         public override void Configuration()
//         {
//             BindSingle<WorldManager>();
//
//             RegisterFSM<WorldManager>(FSMStage.CoreInitStage, StageOrders.World);
//
//             RegisterUpdate<WorldManager>(UpdateStage.World);
//         }
//     }
// }