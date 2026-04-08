namespace App.Common.Input.Runtime
{
    public class InputService
    {
        // public InputActions input { get; private set; }
        //
        // private HashSet<object> _cursorRequiredObjects = new HashSet<object>();
        // private HashSet<object> _cursorVisibilityRequiredObjects = new HashSet<object>();
        //
        // private HashSet<object> _movementBlockers = new HashSet<object>();
        // private HashSet<object> _rotatiobBlockers = new HashSet<object>();
        // private HashSet<object> _interactBlockers = new HashSet<object>();
        // private HashSet<object> _combatBlockers = new HashSet<object>();
        //
        // public InputService()
        // {
        //     input = new InputActions();
        //     input.Enable();
        //
        //     input.UI.Pause.performed += OnInputPause;
        //
        //     input.UIItemsContainer.Disable();
        //
        //     GameStateMachine.beforeStateExitEvent += OnBeforeStateExit; 
        // }
        //
        // private void OnBeforeStateExit(IState state)
        // {
        //     _cursorRequiredObjects.Clear();
        //     _cursorVisibilityRequiredObjects.Clear();
        //
        //     _movementBlockers.Clear();
        //     _rotatiobBlockers.Clear();
        //     _interactBlockers.Clear();
        //     _combatBlockers.Clear();
        //     RefreshCursor();
        //     RefreshInputState();
        //     RefreshCombatInputState();
        // }
        //
        //
        // internal void RefreshCursor()
        // {
        //     RefreshCursorLockStateReasons();
        // }
        //
        // public void AddCharacterRotationBlocker(object obj)
        // {
        //     if (_rotatiobBlockers.Contains(obj))
        //         return;
        //     _rotatiobBlockers.Add(obj);
        //     RefreshInputState();
        // }
        //
        // public void RemoveCharacterRotationBlocker(object obj)
        // {
        //     if (!_rotatiobBlockers.Contains(obj))
        //         return;
        //     _rotatiobBlockers.Remove(obj);
        //     RefreshInputState();
        // }
        //
        // public void AddCharacterMovementBlocker(object obj)
        // {
        //     if (_movementBlockers.Contains(obj))
        //         return;
        //     _movementBlockers.Add(obj);
        //     RefreshInputState();
        // }
        //
        // public void RemoveCharacterMovementBlocker(object obj)
        // {
        //     if (!_movementBlockers.Contains(obj))
        //         return;
        //     _movementBlockers.Remove(obj);
        //     RefreshInputState();
        // }
        //
        // public void AddInteractBlocker(object obj)
        // {
        //     if (_interactBlockers.Contains(obj))
        //         return;
        //     _interactBlockers.Add(obj);
        //     RefreshInputState();
        // }
        //
        // public void RemoveInteractBlocker(object obj)
        // {
        //     if (!_interactBlockers.Contains(obj))
        //         return;
        //     _interactBlockers.Remove(obj);
        //     RefreshInputState();
        // }
        //
        // public void AddCombatBlocker(object obj)
        // {
        //     if (_combatBlockers.Contains(obj))
        //         return;
        //     _combatBlockers.Add(obj);
        //     Debug.Log($"[InputService] AddCombatBlocker: {obj?.GetType().Name ?? "null"} (count: {_combatBlockers.Count})");
        //     RefreshCombatInputState();
        // }
        //
        // public void RemoveCombatBlocker(object obj)
        // {
        //     if (!_combatBlockers.Contains(obj))
        //         return;
        //     _combatBlockers.Remove(obj);
        //     RefreshCombatInputState();
        // }
        //
        // private void RefreshInputState()
        // {
        //     var isMovementActive = _movementBlockers.Count == 0;
        //     if (isMovementActive)
        //     {
        //         input.PlayerMovement.Move.Enable();
        //     }
        //     else
        //     {
        //         input.PlayerMovement.Move.Disable();
        //     }
        //
        //     var isRotationActive = _rotatiobBlockers.Count == 0;
        //     if (isRotationActive)
        //     {
        //         input.PlayerMovement.Look.Enable();
        //     } 
        //     else
        //     {
        //         input.PlayerMovement.Look.Disable();
        //     }
        //
        //     var isInteractActive = _interactBlockers.Count == 0;
        //     if (isInteractActive)
        //     {
        //         input.PlayerInteraction.Enable();
        //     }
        //     else
        //     {
        //         input.PlayerInteraction.Disable();
        //     }
        // }
        //
        // private void RefreshCombatInputState()
        // {
        //     var isCombatActive = _combatBlockers.Count == 0;
        //     if (isCombatActive)
        //     {
        //         input.PlayerCombat.Enable();
        //     }
        //     else
        //     {
        //         input.PlayerCombat.Disable();
        //     }
        // }
        //
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
        //
        // public event System.Action OnPausePressed;
        //
        // private void OnInputPause(InputAction.CallbackContext context)
        // {
        //     OnPausePressed?.Invoke();
        // }
    }
}