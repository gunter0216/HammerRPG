using System.Collections.Generic;
using System.Linq;
using App.Common.Logger.Runtime;
using App.Common.Utilities.Utility.Runtime;
using App.Common.Windows.Runtime;
using UnityEngine;

namespace App.Common.Windows.External
{
    public class WindowManager : IInitSystem, IWindowManager
    {
        private struct WindowInfo
        {
            public IWindowController WindowController;
            public WindowConfig Config;

            public WindowInfo(IWindowController windowController, WindowConfig config)
            {
                WindowController = windowController;
                Config = config;
            }
        }
        
        private readonly Dictionary<WindowNames, WindowInfo> _windows = new();
        private readonly List<WindowInfo> _activeWindows = new(); 
        
        public void Init()
        {
            
        }

        public bool IsAnyOpen()
        {
            return _activeWindows.Count > 0;
        }

        public bool TryPopWindow()
        {
            if (!IsAnyOpen())
            {
                HLogger.LogError("Not have open windows.");
                return false;
            }

            var last = _activeWindows.Last();
            if (!last.Config.CloseOnEscape) // todo
            {
                return false;
            }

            return Close(last.WindowController.GetName());
        }
        
        public bool Registry(IWindowController windowController, WindowConfig config)
        {
            return _windows.TryAdd(windowController.GetName(), new WindowInfo(windowController, config));
        }

        public bool Open(IWindowController window)
        {
            return Open(window.GetName());
        }

        public bool Close(IWindowController window)
        {
            return Close(window.GetName());
        }

        public bool Open(WindowNames windowName)
        {
            return TryOpen(windowName, out _);
        }

        public bool Close(WindowNames windowName)
        {
            return TryClose(windowName, out _);
        }

        public bool TryOpen(WindowNames windowName, out IWindowController windowController)
        {
            windowController = null;
            if (!_windows.TryGetValue(windowName, out var info))
            {
                HLogger.LogError("Window is not register.");
                return false;
            }

            foreach (var activeWindow in _activeWindows)
            {
                if (activeWindow.WindowController.GetName() == windowName)
                {
                    HLogger.LogError("Window is already open.");
                    return false;
                }
            }
            
            windowController = info.WindowController;
            windowController.SetActive(true);
            _activeWindows.Add(info);
            
            info.Config.OnOpened?.Invoke();

            return true;
        }

        public bool TryClose(WindowNames windowName, out IWindowController windowController)
        {
            windowController = null;

            for (int i = 0; i < _activeWindows.Count; ++i)
            {
                var windowInfo = _activeWindows[i];
                windowController = windowInfo.WindowController;
                if (windowController.GetName() == windowName)
                {
                    windowController.SetActive(false);
                    _activeWindows.RemoveAt(i);
                    
                    windowInfo.Config.OnClosed?.Invoke();

                    return true;
                }
            }

            return false;
        }
    }
}