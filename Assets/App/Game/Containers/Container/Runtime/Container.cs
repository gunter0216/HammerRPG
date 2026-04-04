using System.Collections.Generic;
using App.Common.ModuleItem.Runtime;
using App.Game.Containers.Container.Runtime.Data.Model;

namespace App.Game.Containers.Container.Runtime
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

        public void Remove(int slotIndex)
        {
            m_Items[slotIndex] = null;
            m_Data.Items[slotIndex].DataReference = null;
        }

        public void AddItem(IModuleItem item, int slotIndex)
        {
            m_Items[slotIndex] = item;
            m_Data.Items[slotIndex].DataReference = item.ReferenceSelf;
        }
    }
}