// using App.Common.FSM.External;
// using App.Core.Startups.External;
// using App.Core.Startups.External.Attributes;
// using App.Core.Startups.External.Constants;
// using App.Game.Update.External;
//
// namespace App.Game.Inputs.External
// {
//     [Configurator(DIContext.CoreContext)]
//     public class InputConfigurator : Configurator
//     {
//         public override void Configuration()
//         {
//             BindSingle<InputManager>();
//
//             RegisterFSM<InputManager>(FSMStage.CoreInitStage, StageOrders.Input);
//
//             RegisterUpdate<InputManager>(UpdateStage.Input);
//         }
//     }
// }