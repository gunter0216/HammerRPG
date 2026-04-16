namespace App.Core.Startups.External
{
    public enum StageOrders
    {
        // global
        Data = 0,
        DataContainers = 10,
        // start
        StartSceneManager = 100_000,
        // menu
        MenuSceneMenu = 0,
        // core
        World = -100_000,
        ModuleItemsManager = -100_000,
        EcsEventManager = -50_000,
        Input = -10_000,
        GameItems = -10_000,
        Tiles = -5_000,
        CameraFollow = 0,
        Equipment = 0,
        GameManager = 1_000,
        GameMenu = 0,
        Inventory = 0,
        Cheats = 10_000,
        UpdateManager = 100_100,
        // player systems
        EnemyInitSystem = 0,
        PlayerInitSystem = 0,
        AttackSystem = 0,
        HealthSystem = 0,
        PlayAttackAnimationSystem = 0,
        PlayerAttackSystem = 0,
        PlayerMoveSystem = 0,
        WeaponCollisionSystem = 0,
        // end player systems
        TimeManager = 100,
        WindowManager = 100,
        DragItem = 0,
        Container = 0,
        DungeonCreator = 0,
        DungeonController = 100,
        PlayerController = 200,
        ModuleItemsConfigLoader = -10_000,
        FollowIcon = 0
    }
}