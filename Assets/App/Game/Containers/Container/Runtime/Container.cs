using System.Collections.Generic;
using App.Common.ModuleItem.Runtime;
using System;
using App.Game.Containers.Container.Runtime.Data.Model;
using App.Game.Modules.ContainerItem.Runtime.Data;
using App.Game.Modules.ContainerItem.Runtime;

namespace App.Game.Containers.Container.Runtime
{
    public class Container
    {
        private readonly List<IModuleItem> m_Items;
        private readonly ContainerData m_Data;
        private int _columns;

        public int Guid => m_Data.Guid;
        public int Columns => _columns;

        public event Action<int> ItemChanged;
        public event Action<IModuleItem> ItemAdded;

        public List<IModuleItem> Items => m_Items;

        public Container(ContainerData data, List<IModuleItem> items)
        {
            m_Items = items;
            m_Data = data;
            _columns = Math.Max(1, items.Count);
            for (int i = 0; i < m_Items.Count; i++)
                SetItemIndex(m_Items[i], i);
        }

        public bool TrySetGrid(int rows, int columns)
        {
            if (rows <= 0 || columns <= 0 || (long)rows * columns != m_Items.Count)
                return false;

            // Older saves store only the anchor of each item, including items that have since grown.
            var layout = new IModuleItem[m_Items.Count];
            var displaced = new List<IModuleItem>();
            for (int index = 0; index < m_Items.Count; index++)
            {
                var item = m_Items[index];
                if (item == null)
                    continue;
                if (CanPlaceIn(layout, columns, item, index))
                    layout[index] = item;
                else
                    displaced.Add(item);
            }
            foreach (var item in displaced)
            {
                int destination = FindSpace(layout, columns, item);
                if (destination < 0)
                    return false;
                layout[destination] = item;
            }

            _columns = columns;
            for (int index = 0; index < layout.Length; index++)
            {
                if (m_Items[index] != layout[index])
                    WriteSlot(index, layout[index]);
            }
            return true;
        }

        public int GetAnchorIndex(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= m_Items.Count)
                return -1;

            for (int index = slotIndex; index >= 0; index--)
            {
                var item = m_Items[index];
                if (item != null && Covers(item, index, slotIndex, _columns))
                    return index;
            }
            return -1;
        }

        public void Remove(int slotIndex)
        {
            int anchor = GetAnchorIndex(slotIndex);
            if (anchor < 0)
                return;

            SetItemIndex(m_Items[anchor], -1);
            WriteSlot(anchor, null);
            ItemChanged?.Invoke(anchor);
        }

        public int FindSpace(IModuleItem item)
        {
            return FindSpace(m_Items, _columns, item);
        }

        public bool CanPlace(IModuleItem item, int slotIndex, IModuleItem replaced = null)
        {
            return CanPlaceIn(m_Items, _columns, item, slotIndex, replaced);
        }

        public int FindDropIndex(IModuleItem item, int clickedIndex, float offsetX, float offsetY)
        {
            if (item == null || clickedIndex < 0 || clickedIndex >= m_Items.Count ||
                !item.TryGetModule<ContainerItemModule>(out var size))
                return -1;

            int occupiedAnchor = GetAnchorIndex(clickedIndex);
            if (occupiedAnchor >= 0 && m_Items[occupiedAnchor] != item)
                return occupiedAnchor;

            int column = clickedIndex % _columns;
            int row = clickedIndex / _columns;
            int firstColumn = Math.Max(0, column - size.Width + 1);
            int lastColumn = Math.Min(column, _columns - size.Width);
            int firstRow = Math.Max(0, row - size.Height + 1);
            int lastRow = Math.Min(row, m_Items.Count / _columns - size.Height);
            float idealColumn = column + offsetX - (size.Width - 1) * 0.5f;
            float idealRow = row + offsetY - (size.Height - 1) * 0.5f;
            int bestIndex = -1;
            float bestDistance = float.PositiveInfinity;
            for (int y = firstRow; y <= lastRow; y++)
            {
                for (int x = firstColumn; x <= lastColumn; x++)
                {
                    int index = y * _columns + x;
                    float dx = x - idealColumn;
                    float dy = y - idealRow;
                    float distance = dx * dx + dy * dy;
                    if (distance >= bestDistance || !CanPlaceIn(m_Items, _columns, item, index, item))
                        continue;
                    bestDistance = distance;
                    bestIndex = index;
                }
            }
            return bestIndex;
        }

        public bool AddItem(IModuleItem item, int slotIndex)
        {
            if (!CanPlace(item, slotIndex) || m_Items.Contains(item))
                return false;

            WriteSlot(slotIndex, item);
            ItemAdded?.Invoke(item);
            if (ReferenceEquals(m_Items[slotIndex], item))
                ItemChanged?.Invoke(slotIndex);
            return true;
        }

        public bool CanMoveItem(int sourceIndex, int targetIndex)
        {
            if (sourceIndex < 0 || sourceIndex >= m_Items.Count)
                return false;
            var source = m_Items[sourceIndex];
            if (source == null)
                return false;

            int targetAnchor = GetAnchorIndex(targetIndex);
            var target = targetAnchor >= 0 && targetAnchor != sourceIndex ? m_Items[targetAnchor] : null;
            if (target != null)
                targetIndex = targetAnchor;
            if (sourceIndex == targetIndex)
                return true;

            if (!CanPlaceIn(m_Items, _columns, source, targetIndex, source, target))
                return false;
            if (target != null &&
                (!CanPlaceIn(m_Items, _columns, target, sourceIndex, source, target) ||
                 Overlaps(source, targetIndex, target, sourceIndex, _columns)))
                return false;

            return true;
        }

        public bool TryMoveItem(int sourceIndex, int targetIndex)
        {
            if (!CanMoveItem(sourceIndex, targetIndex))
                return false;

            var source = m_Items[sourceIndex];
            int targetAnchor = GetAnchorIndex(targetIndex);
            var target = targetAnchor >= 0 && targetAnchor != sourceIndex ? m_Items[targetAnchor] : null;
            if (target != null)
                targetIndex = targetAnchor;
            if (sourceIndex == targetIndex)
                return true;

            WriteSlot(sourceIndex, target);
            WriteSlot(targetIndex, source);
            ItemChanged?.Invoke(sourceIndex);
            ItemChanged?.Invoke(targetIndex);
            return true;
        }

        private void WriteSlot(int slotIndex, IModuleItem item)
        {
            m_Items[slotIndex] = item;
            m_Data.Items[slotIndex] ??= new ContainerItemData(slotIndex);
            m_Data.Items[slotIndex].DataReference = item?.ReferenceSelf;
            SetItemIndex(item, slotIndex);
        }

        private static int FindSpace(IReadOnlyList<IModuleItem> items, int columns, IModuleItem item)
        {
            for (int index = 0; index < items.Count; index++)
            {
                if (CanPlaceIn(items, columns, item, index))
                    return index;
            }
            return -1;
        }

        private static bool CanPlaceIn(
            IReadOnlyList<IModuleItem> items, int columns, IModuleItem item, int index,
            IModuleItem ignored = null, IModuleItem alsoIgnored = null)
        {
            if (item == null || !item.TryGetModule<ContainerItemModule>(out var size) ||
                index < 0 || index >= items.Count || size.Width > columns - index % columns ||
                size.Height > items.Count / columns - index / columns)
                return false;

            for (int anchor = 0; anchor < items.Count; anchor++)
            {
                var other = items[anchor];
                if (other != null && other != ignored && other != alsoIgnored &&
                    Overlaps(item, index, other, anchor, columns))
                    return false;
            }
            return true;
        }

        private static bool Covers(IModuleItem item, int anchor, int slot, int columns)
        {
            int x = slot % columns - anchor % columns;
            int y = slot / columns - anchor / columns;
            var (width, height) = GetSize(item);
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        private static bool Overlaps(IModuleItem first, int firstAnchor, IModuleItem second, int secondAnchor, int columns)
        {
            var (firstWidth, firstHeight) = GetSize(first);
            var (secondWidth, secondHeight) = GetSize(second);
            int dx = secondAnchor % columns - firstAnchor % columns;
            int dy = secondAnchor / columns - firstAnchor / columns;
            return dx < firstWidth && dx > -secondWidth && dy < firstHeight && dy > -secondHeight;
        }

        private static (int Width, int Height) GetSize(IModuleItem item)
        {
            return item.TryGetModule<ContainerItemModule>(out var size) ? (size.Width, size.Height) : (1, 1);
        }

        private static void SetItemIndex(IModuleItem item, int index)
        {
            if (item != null && item.TryGetDataModule<ContainerItemModuleData>(out var data))
                data.Index = index;
        }
    }
}
