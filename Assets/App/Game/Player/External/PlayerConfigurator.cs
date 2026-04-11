using App.Common.FSM.External;
using App.Core.Startups.External;
using App.Core.Startups.External.Attributes;
using App.Core.Startups.External.Constants;
using App.Game.Update.External;

namespace App.Game.Player.External
{
    [Configurator(DIContext.CoreContext)]
    public class PlayerConfigurator : Configurator
    {
        public override void Configuration()
        {
            BindSingle<AttackSystem>();
            BindSingle<EnemyInitSystem>();
            BindSingle<HealthSystem>();
            BindSingle<PlayAttackAnimationSystem>();
            BindSingle<PlayerAttackSystem>();
            // BindSingle<PlayerInitSystem>();
            // BindSingle<PlayerMoveSystem>();
            BindSingle<WeaponCollisionSystem>();
            BindSingle<PlayerController>();

            RegisterFSM<AttackSystem>(FSMStage.CoreInitStage, StageOrders.AttackSystem);
            RegisterFSM<EnemyInitSystem>(FSMStage.CoreInitStage, StageOrders.EnemyInitSystem);
            RegisterFSM<HealthSystem>(FSMStage.CoreInitStage, StageOrders.HealthSystem);
            RegisterFSM<PlayAttackAnimationSystem>(FSMStage.CoreInitStage, StageOrders.PlayAttackAnimationSystem);
            RegisterFSM<PlayerAttackSystem>(FSMStage.CoreInitStage, StageOrders.PlayerAttackSystem);
            // RegisterFSM<PlayerInitSystem>(FSMStage.CoreInitStage, StageOrders.PlayerInitSystem);
            // RegisterFSM<PlayerMoveSystem>(FSMStage.CoreInitStage, StageOrders.PlayerMoveSystem);
            RegisterFSM<WeaponCollisionSystem>(FSMStage.CoreInitStage, StageOrders.WeaponCollisionSystem);
            RegisterFSM<PlayerController>(FSMStage.CoreInitStage, StageOrders.PlayerController);

            RegisterUpdate<AttackSystem>(UpdateStage.AttackSystem);
            RegisterUpdate<HealthSystem>(UpdateStage.HealthSystem);
            RegisterUpdate<PlayAttackAnimationSystem>(UpdateStage.PlayAttackAnimationSystem);
            RegisterUpdate<PlayerAttackSystem>(UpdateStage.PlayerAttackSystem);
            // RegisterUpdate<PlayerMoveSystem>(UpdateStage.PlayerMoveSystem);
            RegisterUpdate<WeaponCollisionSystem>(UpdateStage.WeaponCollisionSystem);
            RegisterUpdate<PlayerController>(UpdateStage.PlayerController);
        }
    }
}