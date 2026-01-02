using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Events.Runtime;
using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.DragItem.External.Events;
using App.Game.DragItem.External.Model;
using App.Game.Inventory.External.Services;
using App.Game.Inventory.External.ViewModel;
using UniRx;
using UnityEngine;

namespace App.Game.DragItem.External
{
    public class DragItemController : IInitSystem, IDisposable
    {
        private readonly IAssetManager m_AssetManager;
        private readonly PopupCanvas m_PopupCanvas;
        private readonly ISpriteLoader m_SpriteLoader;
        
        private ItemViewModel m_DragView;
        private IItemSlotModel m_ActiveSlot;
        
        private IDisposable m_Disposable;
        private Camera m_Camera;

        public DragItemController(IAssetManager assetManager, PopupCanvas popupCanvas, ISpriteLoader spriteLoader)
        {
            m_AssetManager = assetManager;
            m_PopupCanvas = popupCanvas;
            m_SpriteLoader = spriteLoader;
        }

        public void Init()
        {
            m_Camera = Camera.main;

            CreateView();
            
            EventManager.Subscribe<ItemSlotClickEvent>(OnSlotClick);
            
            m_Disposable = Observable.EveryUpdate()
                .Where(_ => m_DragView != null)
                .Subscribe(_ =>
                {
                    m_DragView.SetPosition(m_Camera.ScreenToWorldPoint(Input.mousePosition));
                });
        }

        private void CreateView()
        {
            var creator = new DragItemViewCreator(m_AssetManager, m_PopupCanvas);
            var view = creator.Create();
            m_DragView = new ItemViewModel(view.Value, m_SpriteLoader);
            m_DragView.SetActive(false);
        }

        private void OnSlotClick(ItemSlotClickEvent data)
        {
            var slot = data.Model;
            var item = slot.GetItem();
            if (m_ActiveSlot != null)
            {
                if (item == null)
                {
                    var modelItem = m_ActiveSlot.GetItem();
                    if (modelItem == null)
                    {
                        HLogger.LogError("Cant remove item.");
                        return;
                    }

                    slot.PlaceItem(modelItem);
                    m_ActiveSlot.DownItem();
                    m_ActiveSlot.RemoveItem();
                    m_DragView.SetActive(false);
                    m_ActiveSlot = null;
                }
                else if (m_ActiveSlot == slot)
                {
                    m_ActiveSlot.DownItem();
                    m_DragView.SetActive(false);
                    m_ActiveSlot = null;
                }
                else
                {
                    SwapItems();
                }
            }
            else
            {
                if (item == null)
                {
                    return;
                }
                else
                {
                    m_ActiveSlot = slot;
                    m_ActiveSlot.UpItem();
                    m_DragView.SetItem(item);
                    m_DragView.SetActive(true);
                }
            }
        }

        private void SwapItems()
        {
            // todo
        }

        public void Dispose()
        {
            m_Disposable?.Dispose();
            EventManager.Unsubscribe<ItemSlotClickEvent>(OnSlotClick);
        }
    }
}