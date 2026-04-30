using System;

namespace App.Common.Windows.External
{
    public class WindowConfig
    {
        internal readonly Action OnClosed;
        internal readonly Action OnOpened;
        internal readonly bool CloseOnEscape;

        public WindowConfig(bool closeOnEscape = true, Action onClosed = null, Action onOpened = null)
        {
            CloseOnEscape = closeOnEscape;
            OnClosed = onClosed;
            OnOpened = onOpened;
        }
    }
}