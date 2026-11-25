using System;
using App.Game.Interactions.External;
using UnityEngine;
using Zenject;

namespace Assets.App.Game.Interactions.Runtime
{
    public class InteractableView : MonoBehaviour, IInteractableView
    {
        private Action _onHoverEnterCallback;
        private Action _onHoverExitCallback;
        private Action _onClickCallback;
        private SpriteHoverHighlight _highlight;
        private InteractionController _interactionController;

        internal SpriteRenderer[] Sprites { get; private set; }

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
            _highlight?.Hide();
            OnHoverExitCallback?.Invoke();
        }

        public void OnClick()
        {
            OnClickCallback?.Invoke();
        }

        [Inject]
        private void Construct([InjectOptional] InteractionController interactionController)
        {
            if (_interactionController == interactionController)
                return;

            _interactionController?.UnregisterView(this);
            _interactionController = interactionController;
            if (isActiveAndEnabled)
                _interactionController?.RegisterView(this);
        }

        private void OnEnable()
        {
            Sprites = GetComponentsInChildren<SpriteRenderer>(true);
            _interactionController?.RegisterView(this);
        }

        private void OnDisable()
        {
            _interactionController?.UnregisterView(this);
            _highlight?.Hide();
        }

        private void LateUpdate()
        {
            if (_interactionController?.CurrentView != this || Sprites.Length == 0)
                return;

            _highlight ??= new SpriteHoverHighlight(transform);
            _highlight.Show(Sprites);
        }

        private void OnDestroy()
        {
            _highlight?.Dispose();
        }
    }
}
