using App.Common.Logger.Runtime;
using App.Common.ModuleItem.External;
using App.Game.StatusBar.External.View;
using UnityEngine;

namespace App.Game.StatusBar.External.Controller
{
    public class StatusBarViewController
    {
        private readonly StatusBarView _view;
        private readonly Camera _camera;
        
        private ModuleItemView _moduleItemView;
        private EntityStatusBarView _statusBarView;

        public ModuleItemView ItemView => _moduleItemView;

        public StatusBarViewController(StatusBarView view)
        {
            _view = view;
            _camera = Camera.main;
        }

        public void OnUpdate()
        {
            if (_statusBarView == null)
            {
                HLogger.LogError("_statusBarView is null");
                return;
            }
            
            _view.transform.position = _camera.WorldToScreenPoint(_statusBarView.Anchor.position);
        }

        public void Activate(ModuleItemView view)
        {
            _moduleItemView = view;

            if (!view.TryGetComponent<EntityStatusBarView>(out _statusBarView))
            {
                HLogger.LogError("Not found EntityStatusBarView");
                return;
            }
            
            _view.SetActive(true);
        }

        public void Deactivate()
        {
            _view.SetActive(false);
        }
    }
}