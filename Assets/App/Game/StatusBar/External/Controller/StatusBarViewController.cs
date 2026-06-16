using App.Common.Logger.Runtime;
using App.Common.ModuleItem.External;
using App.Game.Modules.Health.Runtime;
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
        private HealthModule _healthModule;

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

            if (!_moduleItemView.ModuleItem.TryGetModule<HealthModule>(out _healthModule))
            {
                HLogger.LogError("Not found HealthModule");
                return;
            }

            _healthModule.OnHealthChanged += OnHealthChanged;
            UpdateHealth();
            
            _view.SetActive(true);
        }

        private void OnHealthChanged()
        {
            UpdateHealth();
        }

        private void UpdateHealth()
        {
            _view.SetHealth(_healthModule.Health, _healthModule.MaxHealth);
        }

        public void Deactivate()
        {
            _view.SetActive(false);
            if (_healthModule != null)
            {
                _healthModule.OnHealthChanged -= OnHealthChanged;
            }
        }
    }
}