using System.Collections.Generic;
using System.Linq;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Inventory.Runtime.Config;
using App.Game.ModuleItemType.Runtime.Config.Model;

namespace App.Game.Inventory.External.Group
{
    public class InventoryGroupController
    {
        private const string m_DefaultGroupKey = "Other";
        private readonly IInventoryConfigController m_ConfigController;
        
        private Dictionary<string, IInventoryGroupConfig> m_ItemTypeToGroup;
        private IInventoryGroupConfig m_DefaultGroup;
        
        public InventoryGroupController(IInventoryConfigController configController)
        {
            m_ConfigController = configController;
        }
        
        public bool Initialize()
        {
            var groups = GetGroups();
            m_ItemTypeToGroup = new Dictionary<string, IInventoryGroupConfig>(groups.Count);
            foreach (var group in groups)
            {
                m_ItemTypeToGroup[group.GameType] = group;
            }
            
            m_DefaultGroup = groups.First(x => x.GameType == m_DefaultGroupKey);
            if (m_DefaultGroup == null)
            {
                HLogger.LogError("Default group not found in inventory config");
                return false;
            }

            return true;
        }

        public Optional<IInventoryGroupConfig> GetItemGroup(IModuleItem moduleItem)
        {
            var typeConfig = moduleItem.GetConfigModule<GameItemTypeModuleConfig>();
            if (!typeConfig.HasValue)
            {
                return Optional<IInventoryGroupConfig>.Success(m_DefaultGroup);
            }
            
            if (m_ItemTypeToGroup.TryGetValue(typeConfig.Value.Type, out var group))
            {
                return Optional<IInventoryGroupConfig>.Success(group);
            }
            
            return Optional<IInventoryGroupConfig>.Success(m_DefaultGroup);
        }
        
        public IReadOnlyList<IInventoryGroupConfig> GetGroups()
        {
            return m_ConfigController.GetGroups();
        }
    }
}