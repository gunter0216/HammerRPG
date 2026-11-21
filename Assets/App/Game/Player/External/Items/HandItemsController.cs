using App.Common.AssetSystem.Runtime;
using App.Common.Logger.Runtime;
using App.Game.Modules.Asset.Runtime.Config.Model;
using App.Game.Player.External.Context;
using App.Game.Player.External.View;
using Assets.App.Game.Modules.ModuleItemType.Runtime.Config.Model;
using UnityEngine;

namespace App.Game.Player.External.Items
{
    public class HandItemsController : IHandItemsController
    {
        internal const string IronSwordId = "ironsword";
        internal static readonly int WeaponEquipped = Animator.StringToHash("WeaponEquipped");

        private readonly IAssetManager _assetManager;
        private readonly PlayerContext _context;
        private RigProviderView _rigProvider;

        public HandItemsController(PlayerContext context, IAssetManager assetManager)
        {
            _context = context;
            _assetManager = assetManager;
        }

        public void Initialize()
        {
            _rigProvider = _context.RigProvider;
        }
        
        public void Equip(EquipHandItemInfo info)
        {
            var moduleItem = info.Item;
            if (info.Hand == EHand.Right && moduleItem.Id == IronSwordId)
            {
                _context.View.Animator.SetFloat(WeaponEquipped, 1f);
                return;
            }

            Transform parent = null;
            if (info.Hand == EHand.Left)
            {
                parent = _rigProvider.LeftHand;
            }
            else
            {
                parent = _rigProvider.RightHand;
            }

            if (!moduleItem.TryGetConfigModule<PresentableModuleConfig>(out var assetModuleConfig))
            {
                HLogger.LogError("AssetModuleConfig not found.");
                return;
            }

            var asset = assetModuleConfig.Instantiate();
            if (info.Hand == EHand.Right)
                _context.View.Animator.SetFloat(WeaponEquipped, 0f);

            var itemView = asset.transform;
            itemView.parent = parent;
            itemView.localPosition = Vector3.zero;
            itemView.localRotation = Quaternion.identity;
        }

        public void UnEquip(UnEquipHandItemInfo info)
        {
            if (info.Hand == EHand.Right)
                _context.View.Animator.SetFloat(WeaponEquipped, 0f);
        }
    }
}
