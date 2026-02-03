using System.Collections.Generic;
using App.Common.ModuleItem.Runtime;
using App.Game.Container.Runtime.Data.Model;

namespace App.Game.Container.Runtime
{
    public class Container
    {
        private readonly List<IModuleItem> m_Items;
        private readonly ContainerData m_Data;

        public int Guid => m_Data.Guid;

        public List<IModuleItem> Items => m_Items;

        public Container(ContainerData data, List<IModuleItem> items)
        {
            m_Items = items;
            m_Data = data;
        }

        public void Remove(IModuleItem item, int slotIndex)
        {
            m_Items.Remove(item);
            
            for (int i = 0; i < m_Data.Items.Count; ++i)
            {
                if (m_Data.Items[i].Index == slotIndex)
                {
                    m_Data.Items.RemoveAt(i);
                    return;
                }
            }
        }

        public void AddItem(IModuleItem item, int slotIndex)
        {
            
        }
    }
}