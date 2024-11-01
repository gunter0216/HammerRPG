using App.Common.Utility.Runtime;
using App.Common.Utility.Runtime.Extensions;
using App.Game.Player.Runtime;
using DG.Tweening;
using UnityEngine;

namespace App.Game.Player.External.Animations
{
    public class EntityMeleeWeaponAnimation
    {
        private const float m_Amplitude = 0.4f;
        private const float m_Duration = 0.5f;
        
        public void StartAnimation(EntityView entity, Vector3 attackEventPosition)
        {
            var weapon = entity.Weapon;
            weapon.transform.SetLocalPositionY(m_Amplitude);
            weapon.transform.SetEulerRotateZ(0);
            weapon.gameObject.SetActive(true);
            var seq = DOTween.Sequence();
            seq.Append(TweenHelper.TweenDOLocalMoveY(weapon.transform, -m_Amplitude, m_Duration));
            seq.Join(TweenHelper.TweenDORotateZ(weapon.transform, -90, m_Duration));
            seq.OnComplete(() =>
            {
                weapon.gameObject.SetActive(false);
            });
        }

        public void StopAnimation()
        {
            
        }
    }
}