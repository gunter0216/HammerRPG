using System;
using UnityEngine;

namespace Assets.App.Game.Interactions.Runtime
{
    public class InteractableView : MonoBehaviour, IInteractableView
    {
        private Action _onHoverEnterCallback;
        private Action _onHoverExitCallback;
        private Action _onClickCallback;

        public Action OnHoverEnterCallback
        {
            get => _onHoverEnterCallback;
            set => _onHoverEnterCallback = value;
        }

        public Action OnHoverExitCallback
        {
            get => _onHoverExitCallback;
            set => _onHoverExitCallback = value;
        }

        public Action OnClickCallback
        {
            get => _onClickCallback;
            set => _onClickCallback = value;
        }

        public void OnHoverEnter()
        {
            OnHoverEnterCallback?.Invoke();
        }

        public void OnHoverExit()
        {
            OnHoverExitCallback?.Invoke();
        }

        public void OnClick()
        {
            OnClickCallback?.Invoke();
        }
    }
}