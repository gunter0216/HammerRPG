using App.Common.Logger.Runtime;
using App.Common.ModuleItem.External;
using App.Common.ModuleItem.Runtime;
using App.Game.AI.External.States;
using App.Game.Modules.Health.Runtime;
using App.Game.Player.External.View;
using App.Game.StatusBar.Runtime;
using Game.Project.Gameplay.Weapon.Runtime.DamageHandlers;
using UnityEngine;
using UnityEngine.AI;

namespace App.Game.Dungeon.DungeonCore.External.Controllers
{
    public class AIViewController
    {
        private readonly IStatusBarController _statusBarController;
        private readonly IModuleItem _moduleItem;
        private readonly Transform _view;
        
        private bool _initialized;
        private bool _isActive;
        private NavMeshAgent _agent;
        private AITargetDetector _targetDetector;
        private float _timer;
        private EntityView _entityView;

        private IdleAIState _idleAIState;
        private RunAIState _runAIState;
        private AttackAIState _attackAIState;

        private IAIState _state;
        private EntityView _target;
        private ModuleItemView _moduleItemView;
        private HealthModule _healthModule;
        private DeadAIState _deadAIState;

        public bool IsActive => _isActive;

        public AIViewController(IStatusBarController statusBarController, IModuleItem moduleItem, Transform view)
        {
            _statusBarController = statusBarController;
            _moduleItem = moduleItem;
            _view = view;
        }

        public void Activate()
        {
            if (IsActive)
            {
                return;
            }
            
            Initialize();

            _timer = 0.2f;
            // _agent.SetDestination(target.position);
            _statusBarController.Show(_moduleItemView);

            _isActive = true;
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }
            
            _agent = _view.GetComponent<NavMeshAgent>();
            _entityView = _view.GetComponent<EntityView>();
            _targetDetector = _view.gameObject.AddComponent<AITargetDetector>();
            _moduleItemView = _view.gameObject.AddComponent<ModuleItemView>();
            _moduleItemView.ModuleItem = _moduleItem;

            if (!_view.gameObject.TryGetComponent<HitConsumerView>(out var hitConsumerView))
            {
                HLogger.LogError("HitConsumerView not found.");
                return;
            }
            
            hitConsumerView.SetModuleItem(_moduleItem);

            _idleAIState = new IdleAIState(_entityView);
            _runAIState = new RunAIState(_entityView);
            _deadAIState = new DeadAIState(_entityView);
            _attackAIState = new AttackAIState(_entityView, _moduleItem);

            if (!_moduleItem.TryGetModule<HealthModule>(out _healthModule))
            {
                HLogger.LogError("HealthModule not found.");
                return;
            }

            _healthModule.OnHealthOver += OnHealthOver;
            
            _initialized = true;
        }

        private void OnHealthOver()
        {
            _statusBarController.Hide(_moduleItemView);
            SetState(_deadAIState);
            _isActive = false;
        }

        public void Update()
        {
            if (!_isActive)
            {
                return;
            }
            
            if (_state is IUpdateState updateState)
            {
                updateState.OnUpdate(Time.deltaTime);
            }

            if (_state == _attackAIState && _attackAIState.IsAttack())
            {
                return;
            }
            
            if (_target != null)
            {
                if (_attackAIState.CanAttack(_target))
                {
                    if (_attackAIState != _state)
                    {
                        SetState(_attackAIState);
                    }

                    if (!_attackAIState.IsAttack())
                    {
                        _attackAIState.Attack(_target);
                    }
                    
                    _agent.ResetPath();
                    
                    return;
                }
            }

            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                if (_timer < 0)
                {
                    _timer = 0.2f;
                    UpdateTarget();
                }
            }
        }

        private void UpdateTarget()
        {
            _target = _targetDetector.FindTarget(_entityView);
            if (_target != null)
            {
                _agent.SetDestination(_target.transform.position);
                SetState(_runAIState);
            }
            else
            {
                _agent.ResetPath();
                SetState(_idleAIState);
            }
        }

        private void SetState(IAIState state)
        {
            if (_state == state)
            {
                return;
            }
            
            _state?.Exit();
            _state = state;
            _state.Enter();
        }
    }
}