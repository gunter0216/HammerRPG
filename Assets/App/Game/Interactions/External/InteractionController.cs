using App.Common.Utilities.Utility.Runtime;
using App.Common.Utilities.UtilityUnity.Runtime;
using Assets.App.Game.Interactions.Runtime;
using UnityEngine;
using Input = UnityEngine.Input;

namespace App.Game.Interactions.External
{
    public class InteractionController : IUpdateSystem, IInitSystem
    {
        private IInteractableView _current;
        private Camera _camera;

        public InteractionController()
        {
        }

        public void Init()
        {
            _camera = Camera.main;
        }

        public void OnUpdate()
        {
            IInteractableView newInteractableView = null;

            var ray = _camera.ScreenPointToRay(Input.mousePosition);

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

            if (_current != null && Input.GetMouseButtonDown(0))
            {
                _current.OnClick();
            }
        }
    }
}