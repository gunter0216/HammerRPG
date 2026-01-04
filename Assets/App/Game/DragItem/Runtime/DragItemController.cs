using System;
using App.Common.AssetSystem.Runtime;
using App.Common.Events.Runtime;
using App.Common.Logger.Runtime;
using App.Common.SpriteLoaders.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Game.Canvases.External;
using App.Game.DragItem.Runtime.Events;
using App.Game.DragItem.Runtime.Model;
using UniRx;
using UnityEngine;

namespace App.Game.DragItem.Runtime
{
    public class DragItemController : IInitSystem, IDisposable
    {
        private readonly IAssetManager m_AssetManager;
        private readonly PopupCanvas m_PopupCanvas;
        private readonly ISpriteLoader m_SpriteLoader;
        
        private ItemViewModel m_DragView;
        private IItemSlotModel m_SourceSlot;
        
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
            var targetSlot = data.Model;
            var targetItem = targetSlot.GetItem();
            if (m_SourceSlot != null)
            {
                var sourceSlotItem = m_SourceSlot.GetItem();
                if (sourceSlotItem == null)
                {
                    HLogger.LogError("Source item is null.");
                    return;
                }
                
                if (!targetSlot.CanPlaceItem(sourceSlotItem))
                {
                    return;
                }
                
                if (m_SourceSlot == targetSlot)
                {
                    // опустили на тот же слот
                    m_SourceSlot.DownItem();
                }
                else if (targetItem == null)
                {
                    // положили в пустой слот
                    targetSlot.PlaceItem(sourceSlotItem);
                    m_SourceSlot.DownItem();
                    m_SourceSlot.RemoveItem();
                }
                else
                {
                    // поменяли предметы местами 
                    targetSlot.RemoveItem();
                    targetSlot.PlaceItem(sourceSlotItem);
                    m_SourceSlot.RemoveItem();
                    m_SourceSlot.PlaceItem(targetItem);
                    m_SourceSlot.DownItem();
                }
                
                m_DragView.SetActive(false);
                m_SourceSlot = null;
            }
            else
            {
                if (targetItem == null)
                {
                    // нечего поднимать
                    return;
                }
                else
                {
                    // поднимаем предмет
                    m_SourceSlot = targetSlot;
                    m_SourceSlot.UpItem();
                    m_DragView.SetItem(targetItem);
                    m_DragView.SetActive(true);
                }
            }
        }

        public void Dispose()
        {
            m_Disposable?.Dispose();
            EventManager.Unsubscribe<ItemSlotClickEvent>(OnSlotClick);
        }
    }
}