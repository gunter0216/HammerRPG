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
        IPointerExitHandler,
        IPointerMoveHandler
    {
        private event Action _clicked;
        private event Action _entered;
        private event Action _exited;
        private event Action _move;

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

        public void OnPointerMove(PointerEventData eventData)
        {
            _move?.Invoke();
        }

        public void SetClickListener(Action action)
        {
            _clicked = action;
        }

        public void SetEnterListener(Action action)
        {
            _entered = action;
        }

        public void SetExitListener(Action action)
        {
            _exited = action;
        }
    }
}