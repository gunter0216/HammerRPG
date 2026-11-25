using System.Collections.Generic;
using App.Common.Utilities.UtilityUnity.Runtime;
using Assets.App.Game.Interactions.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace App.Game.Interactions.External
{
    public class InteractionController
    {
        private readonly HashSet<InteractableView> _views = new();
        private Camera _camera;

        public InteractableView CurrentView { get; private set; }

        public void Initialize()
        {
            _camera = Camera.main;
        }

        public bool OnLeftClick()
        {
            if (CurrentView != null && CurrentView.isActiveAndEnabled && CurrentView.OnClickCallback != null)
            {
                CurrentView.OnClick();
                return true;
            }

            return false;
        }

        public void OnUpdate(bool canInteract)
        {
            if (!canInteract || _camera == null || Mouse.current == null)
            {
                SetHoveredView(null);
                return;
            }

            var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            var view = FindSpriteView(ray);
            if (view == null && RayCastHelper.RaycastNonAllocSingle(ray, out RaycastHit hit, 100, ~0))
                view = hit.collider.GetComponentInParent<InteractableView>();

            SetHoveredView(view != null && view.isActiveAndEnabled ? view : null);
        }

        internal void RegisterView(InteractableView view) => _views.Add(view);

        internal void UnregisterView(InteractableView view)
        {
            _views.Remove(view);
            if (CurrentView == view)
                SetHoveredView(null);
        }

        private void SetHoveredView(InteractableView view)
        {
            if (CurrentView == view)
                return;

            var previous = CurrentView;
            CurrentView = view;
            if (previous != null)
                previous.OnHoverExit();

            if (view != null && CurrentView == view)
                view.OnHoverEnter();
        }

        private InteractableView FindSpriteView(Ray ray)
        {
            InteractableView closest = null;
            SpriteRenderer frontSprite = null;
            float distance = float.PositiveInfinity;

            foreach (var view in _views)
            {
                foreach (var sprite in view.Sprites)
                {
                    if (!SpriteHoverHighlight.IsVisible(sprite) || (_camera.cullingMask & (1 << sprite.gameObject.layer)) == 0 ||
                        !sprite.bounds.IntersectRay(ray, out var hitDistance) || hitDistance > _camera.farClipPlane)
                        continue;

                    if (frontSprite != null)
                    {
                        int sorting = SpriteHoverHighlight.CompareSorting(sprite, frontSprite);
                        if (sorting < 0 || sorting == 0 && hitDistance >= distance)
                            continue;
                    }

                    closest = view;
                    frontSprite = sprite;
                    distance = hitDistance;
                }
            }

            return closest;
        }
    }
}
