using App.Common.ModuleItem.External;
using App.Common.ModuleItem.Runtime;
using App.Game.AI.External.States;
using App.Game.Player.External.View;
using UnityEngine;
using UnityEngine.AI;

namespace App.Game.Dungeon.DungeonCore.External.Controllers
{
    public class AIViewController
    {
        private readonly IModuleItem _enemy;
        private readonly Transform _view;
        
        private bool _initialized;
        private bool _isActive;
        private NavMeshAgent _agent;
        private AITargetDetector _targetDetector;
        private float _timer;
        private EntityView _entityView;

        private IdleAIState _idleAIState;
        private RunAIState _runAIState;
        
        private IAIState _state;
        
        public bool IsActive => _isActive;


        public AIViewController(IModuleItem enemy, Transform view)
        {
            _enemy = enemy;
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
            _entityView.ModuleItemView = _view.gameObject.AddComponent<ModuleItemView>();
            _entityView.ModuleItemView.ModuleItem = _enemy;

            _idleAIState = new IdleAIState(_entityView);
            _runAIState = new RunAIState(_entityView);
            
            _initialized = true;
        }

        public void Update()
        {
            if (!_isActive)
            {
                return;
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
            var target = _targetDetector.FindTarget(_entityView);
            if (target != null)
            {
                _agent.SetDestination(target.transform.position);
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
            _state?.Exit();
            _state = state;
            _state.Enter();
        }
    }
}