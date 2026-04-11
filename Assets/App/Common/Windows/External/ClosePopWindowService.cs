using App.Common.Utilities.Utility.Runtime;
using UnityEngine;

namespace App.Common.Windows.External
{
    public class ClosePopWindowService : IUpdateSystem
    {
        private readonly WindowManager _windowManager;

        public ClosePopWindowService(WindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public void OnUpdate()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                if (_windowManager.IsAnyOpen())
                {
                    _windowManager.TryPopWindow();
                }
            }
        }
    }
}