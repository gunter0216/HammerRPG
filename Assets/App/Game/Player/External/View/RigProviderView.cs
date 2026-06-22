using UnityEngine;

namespace App.Game.Player.External.View
{
    public class RigProviderView : MonoBehaviour
    {
        [SerializeField] private Transform _leftHand;
        [SerializeField] private Transform _rightHand;
        
        public Transform LeftHand => _leftHand;
        public Transform RightHand => _rightHand;
    }
}