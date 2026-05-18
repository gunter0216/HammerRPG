using UnityEngine;

namespace Game.Project.Gameplay.Move.External.View
{
    public class MoveAnimationPresenter
    {
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        
        private readonly Transform _transform;
        
        private Animator _animator;
        private float _animatorVelocity;
        
        private bool _hasMoveX;
        private bool _hasMoveY;

        private float _sprinting;
        
        public MoveAnimationPresenter(Transform transform)
        {
            _transform = transform;
        }

        public void Initialize()
        {
            _animator = _transform.GetComponentInChildren<Animator>();
            
            if (_animator != null && _animator.runtimeAnimatorController != null)
            {
                var parameters = _animator.parameters;
                _hasMoveX = HasParameter(parameters, MoveX);
                _hasMoveY = HasParameter(parameters, MoveY);
            }
        }
        
        private bool HasParameter(AnimatorControllerParameter[] parameters, int hash)
        {
            foreach (var param in parameters)
            {
                if (param.nameHash == hash)
                    return true;
            }
            return false;
        }

        // public void OnUpdate(Vector3 inputVelocity, float deltaTime)
        // {
        //     if (_animator == null) return;
        //
        //     const float velocitySmoothing = 4f;
        //
        //     var velocity = new Vector2(inputVelocity.x, Mathf.Abs(inputVelocity.z));
        //     velocity.x = Mathf.Abs(velocity.x) < 0.5f ? 0f : velocity.x / Mathf.Abs(velocity.x);
        //     velocity.y = Mathf.Abs(velocity.y) < 0.5f ? 0f : velocity.y / Mathf.Abs(velocity.y);
        //
        //     _animatorVelocity = Vector2.Lerp(_animatorVelocity, velocity,
        //         ExpDecayAlpha(velocitySmoothing, deltaTime));
        //
        //     if (_hasMoveX) _animator.SetFloat(MoveX, _animatorVelocity.x);
        //     if (_hasMoveY) _animator.SetFloat(MoveY, _animatorVelocity.y);
        // }
        
        public void OnUpdate(Vector3 inputVelocity, float deltaTime)
        {
            if (_animator == null)
                return;

            const float velocitySmoothing = 4f;


            var hasMovement =
                Mathf.Abs(inputVelocity.x) > 0.01f ||
                Mathf.Abs(inputVelocity.z) > 0.01f;

            float velocity = hasMovement ? 1f : 0f;

            _animatorVelocity = Mathf.Lerp(
                _animatorVelocity,
                velocity,
                ExpDecayAlpha(velocitySmoothing, deltaTime));
            
            // if (_hasMoveX)
            //     _animator.SetFloat(MoveX, 0);

            if (_hasMoveY)
                _animator.SetFloat(MoveY, _animatorVelocity);
        }

        public float ExpDecayAlpha(float speed, float deltaTime)
        {
            return 1 - Mathf.Exp(-speed * deltaTime);
        }
    }
}