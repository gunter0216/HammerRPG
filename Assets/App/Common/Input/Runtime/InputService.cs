using System.Collections.Generic;
using InputSystem;

namespace App.Common.Input.Runtime
{
    public class InputService : IInputService
    {
        public InputActions Input { get; private set; }
        
        private readonly HashSet<object> _movementBlockers = new HashSet<object>();
        private readonly HashSet<object> _combatBlockers = new HashSet<object>();
        private readonly HashSet<object> _uiBlockers = new HashSet<object>();
        
        public InputService()
        {
            Input = new InputActions();
            Input.Enable();
        }
        
        public void AddMovementBlocker(object obj)
        {
            if (!_movementBlockers.Add(obj))
                return;
            RefreshInputState();
        }
        
        public void RemoveMovementBlocker(object obj)
        {
            if (!_movementBlockers.Contains(obj))
                return;
            _movementBlockers.Remove(obj);
            RefreshInputState();
        }
        
        public void AddUIBlocker(object obj)
        {
            if (_uiBlockers.Contains(obj))
                return;
            _uiBlockers.Add(obj);
            RefreshUIState();
        }

        public void RemoveUIBlocker(object obj)
        {
            if (!_uiBlockers.Contains(obj))
                return;
            _uiBlockers.Remove(obj);
            RefreshUIState();
        }

        public void AddCombatBlocker(object obj)
        {
            if (_combatBlockers.Contains(obj))
                return;
            _combatBlockers.Add(obj);
            RefreshCombatInputState();
        }

        public void RemoveCombatBlocker(object obj)
        {
            if (!_combatBlockers.Contains(obj))
                return;
            _combatBlockers.Remove(obj);
            RefreshCombatInputState();
        }

        private void RefreshUIState()
        {
            if (_uiBlockers.Count <= 0)
            {
                Input.UI.Enable();
            }
            else
            {
                Input.UI.Disable();
            }
        }

        private void RefreshInputState()
        {
            if (_movementBlockers.Count <= 0)
            {
                Input.Movement.Enable();
            }
            else
            {
                Input.Movement.Disable();
            }
        }
        
        private void RefreshCombatInputState()
        {
            if (_combatBlockers.Count <= 0)
            {
                Input.Combat.Enable();
            }
            else
            {
                Input.Combat.Disable();
            }
        }
        
        // public void AddCursorFullRequirements(object obj, CancellationToken destroyToken = default)
        // {
        //     if (destroyToken != default)
        //     {
        //         destroyToken.Register(() =>
        //         {
        //             RemoveCursorFullRequirements(obj);
        //         });
        //     }
        //
        //     AddCursorUnlockRequired(obj);
        //     AddCursorVisRequired(obj);
        // }
        //
        // public void RemoveCursorFullRequirements(object gameObject)
        // {
        //     RemoveCursorUnlockRequired(gameObject);
        //     RemoveCursorVisRequired(gameObject);
        // }
        //
        // public void AddCursorUnlockRequired(object requiredCursor)
        // {
        //     if (_cursorRequiredObjects.Contains(requiredCursor))
        //         return;
        //     _cursorRequiredObjects.Add(requiredCursor);
        //     Debug.Log($"[InputService] AddCursorUnlock: {requiredCursor?.GetType().Name ?? "null"} (count: {_cursorRequiredObjects.Count})");
        //     RefreshCursorLockStateReasons();
        // }
        //
        // public void RemoveCursorUnlockRequired(object requiredCursor)
        // {
        //     if (!_cursorRequiredObjects.Contains(requiredCursor))
        //         return;
        //     _cursorRequiredObjects.Remove(requiredCursor);
        //     RefreshCursorLockStateReasons();
        // }
        //
        // public void AddCursorVisRequired(object requiredCursor)
        // {
        //     if (_cursorVisibilityRequiredObjects.Contains(requiredCursor))
        //         return;
        //     _cursorVisibilityRequiredObjects.Add(requiredCursor);
        //     Debug.Log($"[InputService] AddCursorVis: {requiredCursor?.GetType().Name ?? "null"} (count: {_cursorVisibilityRequiredObjects.Count})");
        //     RefreshCursorLockStateReasons();
        // }
        //
        // public void RemoveCursorVisRequired(object requiredCursor)
        // {
        //     if (!_cursorVisibilityRequiredObjects.Contains(requiredCursor))
        //         return;
        //     _cursorVisibilityRequiredObjects.Remove(requiredCursor);
        //     RefreshCursorLockStateReasons();
        // }
        //
        // private void RefreshCursorLockStateReasons()
        // {
        //     Cursor.lockState = _cursorRequiredObjects.Count > 0
        //         ? CursorLockMode.Confined
        //         : CursorLockMode.Locked;
        //
        //     Cursor.visible = _cursorVisibilityRequiredObjects.Count > 0;
        // }
    }
}