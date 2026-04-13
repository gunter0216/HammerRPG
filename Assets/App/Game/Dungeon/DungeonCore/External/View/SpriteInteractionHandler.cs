using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Game.Dungeon.DungeonCore.External.View
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class SpriteInteractionHandler : MonoBehaviour,
        IPointerClickHandler,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        private event Action _clicked;
        private event Action _entered;
        private event Action _exited;

        public void OnPointerClick(PointerEventData eventData)
        {
            _clicked?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _entered?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _exited?.Invoke();
        }

        public void AddClickListener(Action onButtonClick)
        {
            _clicked = onButtonClick;
        }
    }
}