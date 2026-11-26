using App.Common.ModuleItem.External;
using App.Common.ModuleItem.Runtime.Config;
using UnityEngine;

namespace App.Game.Dungeon.DungeonCore.External.View
{
    public sealed class CreatingModuleItemView : ModuleItemView
    {
        [SerializeField, HideInInspector] private string _objectId;
        [SerializeField] private ModuleItemGameConfig _config;

        public ModuleItemGameConfig Config => _config;
        public string ObjectId => _objectId;
    }
}
