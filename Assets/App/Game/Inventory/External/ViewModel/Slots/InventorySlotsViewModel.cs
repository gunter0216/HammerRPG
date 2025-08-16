using System.Collections.Generic;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Pool.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Inventory.External.Services;
using App.Game.Inventory.Runtime.Config;
using App.Game.SpriteLoaders.Runtime;
using App.Generation.DungeonGenerator.Runtime.Matrix;

namespace App.Game.Inventory.External.ViewModel
{
    public class InventorySlotsViewModel
    {
        private readonly IInventoryConfigController m_ConfigController;
        private readonly InventorySlotViewCreator m_SlotViewCreator;
        private readonly InventoryItemsController m_ItemsController;
        private readonly InventoryItemViewCreator m_ItemViewCreator;
        private readonly ISpriteLoader m_SpriteLoader;
        
        private Matrix<InventorySlotViewModel> m_SlotsMatrix;
        
        private ListPool<InventoryItemViewModel> m_ItemsPool;
        private List<InventoryItemViewModel> m_ActiveItems;
        
        private InventoryItemViewModel m_SelectedItem;

        public InventorySlotsViewModel(
            IInventoryConfigController configController, 
            InventorySlotViewCreator slotViewCreator, 
            InventoryItemsController itemsController, 
            InventoryItemViewCreator itemViewCreator, 
            ISpriteLoader spriteLoader)
        {
            m_ConfigController = configController;
            m_SlotViewCreator = slotViewCreator;
            m_ItemsController = itemsController;
            m_ItemViewCreator = itemViewCreator;
            m_SpriteLoader = spriteLoader;
        }

        public void Initialize()
        {
            CreateSlots();
            var maxItems = m_ConfigController.GetRows() * m_ConfigController.GetCols();
            var capacity = maxItems / 2;

            m_ItemsPool = new ListPool<InventoryItemViewModel>(
                createFunc: Create, 
                capacity: capacity,
                maxItems: maxItems,
                actionOnGet: item => item.SetActive(true),
                actionOnRelease: item => item.SetActive(false));
            m_ActiveItems = new List<InventoryItemViewModel>(capacity);
        }

        private Optional<InventoryItemViewModel> Create()
        {
            var itemView = m_ItemViewCreator.Create();
            if (!itemView.HasValue)
            {
                HLogger.LogError("Failed to create InventoryItemView.");
                return Optional<InventoryItemViewModel>.Empty;
            }
            
            var viewModel = new InventoryItemViewModel(itemView.Value, m_SpriteLoader, OnItemClick);
            return Optional<InventoryItemViewModel>.Success(viewModel);
        }

        private void CreateSlots()
        {
            var rows = m_ConfigController.GetRows();
            var columns = m_ConfigController.GetCols();
            m_SlotsMatrix = new Matrix<InventorySlotViewModel>(columns, rows);
            for (int row = 0; row < rows; ++row)
            {
                for (int col = 0; col < columns; ++col)
                {
                    var view = m_SlotViewCreator.Create();
                    if (!view.HasValue)
                    {
                        HLogger.LogError($"Failed to create slot view");
                        return;
                    }
                    
                    var viewModel = new InventorySlotViewModel(
                        view.Value,
                        row,
                        col,
                        OnSlotClick);
                    viewModel.Initialize();
                    m_SlotsMatrix.SetCell(row, col, viewModel);
                }
            }
        }

        public void ShowGroup(IInventoryGroupConfig group)
        {
            HideAllItems();
            var items = m_ItemsController.GetItemsByGroup(group);
            foreach (var item in items)
            {
                ShowItem(item);
            }
        }

        public void ShowItem(InventoryItem item)
        {
            var itemViewModel = m_ItemsPool.Get();
            if (!itemViewModel.HasValue)
            {
                HLogger.LogError("Failed to acquire InventoryItemViewModel from pool.");
                return;
            }
                
            itemViewModel.Value.SetActive(true);
            itemViewModel.Value.SetItem(item);
            var slot = m_SlotsMatrix.GetCell(item.Data.PositionY, item.Data.PositionX);
            var position = slot.GetLocalPosition();
            itemViewModel.Value.SetPosition(position);
            slot.SetItem(itemViewModel.Value);
            m_ActiveItems.Add(itemViewModel.Value);
        }

        private void OnSlotClick(InventorySlotViewModel slot)
        {
            if (m_SelectedItem == null)
            {
                return;
            }
            
            if (slot.Item != null)
            {
                if (slot.Item == m_SelectedItem)
                {
                    DropSelectedItemInTheSameSlot();
                }
                
                return;
            }
            
            var itemData = m_SelectedItem.Item.Data;
            var prevSlot = m_SlotsMatrix.GetCell(itemData.PositionY, itemData.PositionX);
            prevSlot.SetItem(null);
            
            slot.SetItem(m_SelectedItem);
            
            itemData.PositionX = slot.Col;
            itemData.PositionY = slot.Row;
            
            DropSelectedItemInSlot(slot);
        }

        public void OnWindowClosed()
        {
            if (m_SelectedItem != null)
            {
                DropSelectedItemInTheSameSlot();
            }
        }

        private void DropSelectedItemInTheSameSlot()
        {
            var itemData = m_SelectedItem.Item.Data;
            var slot = m_SlotsMatrix.GetCell(itemData.PositionY, itemData.PositionX);
            DropSelectedItemInSlot(slot);
        }

        private void DropSelectedItemInSlot(InventorySlotViewModel slot)
        {
            var position = slot.GetLocalPosition();
            m_SelectedItem.SetPosition(position);
            m_SelectedItem.SetScale(1.0f);
            m_SelectedItem.SetButtonActive(true);
            m_SelectedItem = null;
        } 

        private void OnItemClick(InventoryItemViewModel itemViewModel)
        {
            if (m_SelectedItem != null)
            {
                return;
            }

            m_SelectedItem = itemViewModel;
            m_SelectedItem.SetAsLastSibling();
            m_SelectedItem.SetButtonActive(false);
            m_SelectedItem.SetScale(1.25f);
        }

        private void HideAllItems()
        {
            foreach (var slot in m_SlotsMatrix)
            {
                slot.SetItem(null);
            }
            
            foreach (var item in m_ActiveItems)
            {
                m_ItemsPool.Release(item);
            }
            
            m_ActiveItems.Clear();
        }
    }
}