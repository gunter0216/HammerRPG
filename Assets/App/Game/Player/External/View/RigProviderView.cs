using UnityEngine;

namespace App.Game.Player.External.View
{
    public class RigProviderView : MonoBehaviour
    {
        [SerializeField] private Transform _leftHand;
        [SerializeField] private Transform _rightHand;
        [SerializeField] private SpriteRenderer _weaponNear;
        [SerializeField] private SpriteRenderer _weaponFar;
        [SerializeField] private SpriteRenderer _shieldNear;
        [SerializeField] private SpriteRenderer _shieldFar;
        
        public Transform LeftHand => _leftHand;
        public Transform RightHand => _rightHand;

        public bool HasWeaponRenderers => _weaponNear != null && _weaponFar != null;

        public void SetWeaponSprites(Sprite nearSprite, Sprite farSprite)
        {
            _weaponNear.sprite = nearSprite;
            _weaponFar.sprite = farSprite;
        }

        public void SetShieldSprites(Sprite nearSprite, Sprite farSprite)
        {
            if (_shieldNear != null)
                _shieldNear.sprite = nearSprite;
            if (_shieldFar != null)
                _shieldFar.sprite = farSprite;
        }
    }
}
