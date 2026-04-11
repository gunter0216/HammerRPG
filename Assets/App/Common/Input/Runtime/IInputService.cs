using System.Threading;
using InputSystem;

namespace App.Common.Input.Runtime
{
    public interface IInputService
    {
        public InputActions Input { get; }
        // movement
        void AddMovementBlocker(object obj);
        void RemoveMovementBlocker(object obj);
        // ui
        void AddUIBlocker(object obj);
        void RemoveUIBlocker(object obj);
        // combat
        void AddCombatBlocker(object obj);
        void RemoveCombatBlocker(object obj);
        // cursor
        // void AddCursorFullRequirements(object obj, CancellationToken destroyToken = default);
        // void RemoveCursorFullRequirements(object gameObject);
        // void AddCursorUnlockRequired(object requiredCursor);
        // void RemoveCursorUnlockRequired(object requiredCursor);
        // void AddCursorVisRequired(object requiredCursor);
        // void RemoveCursorVisRequired(object requiredCursor);
    }
}