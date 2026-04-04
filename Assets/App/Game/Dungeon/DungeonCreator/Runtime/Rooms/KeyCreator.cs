using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Game.Dungeon.DungeonCreator.Runtime.Chest;
using App.Generation.DungeonGenerator.Runtime.DungeonGenerators.Generation.Common;

namespace App.Game.Dungeon.DungeonCreator.Runtime.Rooms
{
    public class KeyCreator
    {
        private readonly IModuleItemsManager _moduleItemsManager;
        private readonly Dictionary<DungeonKeyData, IModuleItem> _keys = new();

        public KeyCreator(IModuleItemsManager moduleItemsManager)
        {
            _moduleItemsManager = moduleItemsManager;
        }

        public IModuleItem Create(DungeonKeyData dungeonKey)
        {
            if (dungeonKey == null)
            {
                HLogger.LogError("Key is empty");
                return null;
            }
            
            if (_keys.TryGetValue(dungeonKey, out var item))
            {
                return item;
            }

            var moduleItem = _moduleItemsManager.Create("IronKey");
            if (!moduleItem.HasValue)
            {
                HLogger.LogError($"Cant create moduleItem");
                return null;
            }

            var keyModuleData = new KeyModuleData(dungeonKey);
            if (!moduleItem.Value.AddDataModule(keyModuleData))
            {
                HLogger.LogError("Cant add key data");
                return null;
            }
            
            _keys.Add(dungeonKey, moduleItem.Value);
            
            return moduleItem.Value;
        }
    }
}