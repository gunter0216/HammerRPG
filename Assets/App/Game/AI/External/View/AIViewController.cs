using App.Common.ModuleItem.External;
using App.Common.ModuleItem.Runtime;
using App.Game.AI.External.View;
using App.Game.AI.External.States;
using App.Game.Modules.Health.Runtime;
using App.Game.Modules.Move.Runtime;
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
        private readonly MoveModuleSystem _moveModules;
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
        private EnemyCombat2D _combat2D;

        public bool IsActive => _isActive && _view != null;

        public AIViewController(IStatusBarController statusBarController, MoveModuleSystem moveModules,
            IModuleItem moduleItem, Transform view)
        {
            _statusBarController = statusBarController;
            _moveModules = moveModules;
            _moduleItem = moduleItem;
            _view = view;
        }

        public bool Activate()
        {
            if (IsActive)
                return true;
            if (!_initialized && !Initialize())
                return false;

            _timer = 0.2f;
            _statusBarController.Show(_moduleItemView);

            _isActive = true;
            return true;
        }

        private bool Initialize()
        {
            if (!_view.TryGetComponent(out _entityView) ||
                !_view.TryGetComponent<HitConsumerView>(out var hitConsumerView) ||
                !_moduleItem.TryGetModule<HealthModule>(out _healthModule))
                return false;

            if (_view.TryGetComponent<Rigidbody2D>(out var body))
            {
                if (!_moveModules.TryGetModule(_moduleItem, out var move))
                    return false;

                _combat2D = new EnemyCombat2D(_moduleItem, _entityView, body, move);
                if (!_combat2D.Initialize())
                    return false;
            }
            else
            {
                _agent = _view.GetComponent<NavMeshAgent>();
                _targetDetector = _view.gameObject.AddComponent<AITargetDetector>();
                _idleAIState = new IdleAIState(_entityView);
                _runAIState = new RunAIState(_entityView);
                _deadAIState = new DeadAIState(_entityView);
                _attackAIState = new AttackAIState(_entityView, _moduleItem);
            }

            _moduleItemView = _view.gameObject.AddComponent<ModuleItemView>();
            _moduleItemView.ModuleItem = _moduleItem;
            hitConsumerView.SetModuleItem(_moduleItem);
            _healthModule.OnHealthOver += OnHealthOver;
            
            _initialized = true;
            return true;
        }

        private void OnHealthOver()
        {
            _healthModule.OnHealthOver -= OnHealthOver;
            _statusBarController.Hide(_moduleItemView);
            if (_combat2D != null)
                _combat2D.Die();
            else
                SetState(_deadAIState);
            _isActive = false;
        }

        public void Update()
        {
            if (!IsActive)
            {
                return;
            }

            if (_combat2D != null)
            {
                _combat2D.Update();
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
