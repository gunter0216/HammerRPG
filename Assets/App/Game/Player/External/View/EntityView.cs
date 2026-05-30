using App.Common.ModuleItem.External;
using UnityEngine;

namespace App.Game.Player.External.View
{
    public class EntityView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public Transform Transform => transform;
        public Animator Animator
        {
            get
            {
                if (_animator == null)
                {
                    _animator = gameObject.GetComponentInChildren<Animator>();
                }

                return _animator;
            }
        }
    }
}