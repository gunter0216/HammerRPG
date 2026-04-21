using System;

namespace App.Game.Update.External
{
    public enum UpdateStage
    {
        CameraFollow = 100,
        Cheats = 0,
        EcsEventManager = Int32.MaxValue, 
        GameMenu = -1000,
        Input = -100_000,
        Inventory = 0,
        World = 0,
        
        AttackSystem = 100,
        HealthSystem = 100,
        PlayAttackAnimationSystem = 200,
        PlayerAttackSystem = 0,
        PlayerMoveSystem = 100,
        WeaponCollisionSystem = 300,
        
        TimeManager = 1000,
        PopWindow = -100,
        CharacterOpenSystem = 0,
        PlayerController = 0,
        FollowIcon = 0,
        HUD = 0
    }
}