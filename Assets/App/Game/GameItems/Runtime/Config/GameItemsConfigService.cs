using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.ModuleItem.Runtime.Config.Interfaces;
using App.Common.Utility.Runtime;

namespace App.Game.GameTiles.External.Config.Model
{
    public class GameItemsConfigService : IGameItemsConfigService
    {
        private IReadOnlyList<IModuleItemConfig> m_Items;
        private Dictionary<string, List<IModuleItemConfig>> m_TypeToItems;
        
        public void SetItems(IReadOnlyList<IModuleItemConfig> items)
        {
            m_Items = items;
            m_TypeToItems = new Dictionary<string, List<IModuleItemConfig>>();
            foreach (var item in m_Items)
            {
                if (!item.TryGetModule<GameItemTypeModuleConfig>(out var typeModule))
                {
                    HLogger.LogError($"Not found type for item {item.Id}");
                    continue;
                }

                if (!m_TypeToItems.TryGetValue(typeModule.Type, out var typeItems))
                {
                    typeItems = new List<IModuleItemConfig>(1);
                    m_TypeToItems.Add(typeModule.Type, typeItems);
                }
                
                typeItems.Add(item);
            }
        }

        public Optional<IReadOnlyList<IModuleItemConfig>> GetItemsByType(string type)
        {
            if (!m_TypeToItems.TryGetValue(type, out var typeItems))
            {
                return Optional<IReadOnlyList<IModuleItemConfig>>.Fail();
            }
            
            return Optional<IReadOnlyList<IModuleItemConfig>>.Success(typeItems);
        }
    }
}