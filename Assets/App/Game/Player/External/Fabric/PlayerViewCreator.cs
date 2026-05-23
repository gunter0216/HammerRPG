using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.External;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Player.External.View;

namespace App.Game.Player.External
{
    public class PlayerViewCreator
    {
        private const string _playerAssetKey = "Player";
        
        private readonly IAssetManager _assetManager;
        
        public PlayerViewCreator(IAssetManager assetManager)
        {
            _assetManager = assetManager;
        }

        public Optional<EntityView> Create(IModuleItem player)
        {
            var entityView = _assetManager.InstantiateSync<EntityView>(new StringKeyEvaluator(_playerAssetKey));
            if (!entityView.HasValue)
            {
                HLogger.LogError("cant create player");
                return Optional<EntityView>.Fail();
            }

            var moduleItemView = entityView.Value.gameObject.AddComponent<ModuleItemView>();
            entityView.Value.ModuleItemView = moduleItemView;
            moduleItemView.ModuleItem = player;
            
            // var view = entityView.Value;
            // view.Weapon.gameObject.SetActive(false);

            // var weaponView = entity.View.Weapon.GetComponent<WeaponView>();
            // if (weaponView != null)
            // {
            //     weaponView.SetOnTriggerEnter2D((other) =>
            //     {
            //         if (other.TryGetComponent<EntityView>(out var attackedEntityView))
            //         {
            //             m_WeaponCollisionEventPool.Trigger(new WeaponCollisionEvent(
            //                 attackerEntityId: playerEntity,
            //                 attackedEntityId: attackedEntityView.Entity,
            //                 other: other));
            //         }
            //     });
            // }
            
            return Optional<EntityView>.Success(entityView.Value);
        }
    }
}