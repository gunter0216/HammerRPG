using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.External;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Player.External.View;
using Game.Project.Gameplay.Weapon.Runtime.DamageHandlers;

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
            moduleItemView.ModuleItem = player;
            
            if (!entityView.Value.gameObject.TryGetComponent<HitConsumerView>(out var hitConsumerView))
            {
                HLogger.LogError("HitConsumerView not found.");
                return Optional<EntityView>.Fail();
            }
            
            hitConsumerView.SetModuleItem(player);
            
            return Optional<EntityView>.Success(entityView.Value);
        }
    }
}