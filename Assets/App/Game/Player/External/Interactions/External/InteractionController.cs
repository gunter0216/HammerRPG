using App.Common.Input.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Common.Utilities.UtilityUnity.Runtime;
using Assets.App.Game.Interactions.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.Game.Interactions.External
{
    public class InteractionController
    {
        private IInteractableView _current;
        private Camera _camera;

        public InteractionController()
        {
        }

        public void Initialize()
        {
            _camera = Camera.main;
        }

        public bool OnLeftClick()
        {
            if (_current != null)
            {
                _current.OnClick();
                return true;
            }

            return false;
        }

        public void OnUpdate()
        {
            IInteractableView newInteractableView = null;

            var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (RayCastHelper.RaycastNonAllocSingle(ray, out RaycastHit hit, 100, ~0))
            {
                newInteractableView = hit.collider.GetComponentInParent<IInteractableView>();
            }

            if (_current != newInteractableView)
            {
                _current?.OnHoverExit();

                _current = newInteractableView;

                _current?.OnHoverEnter();
            }
        }
    }
}