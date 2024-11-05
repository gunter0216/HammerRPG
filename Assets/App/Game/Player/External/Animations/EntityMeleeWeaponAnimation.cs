using App.Common.Utility.Runtime;
using App.Common.Utility.Runtime.Extensions;
using App.Game.Player.External.View;
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
            var direction = ((Vector2)attackEventPosition - (Vector2)entity.Transform.position).normalized;
            float angle = 0.0f;
            float from = 0.0f;
            float to = 0.0f;
            if (direction.x > 0)
            {
                entity.WeaponRoot.SetLocalScaleX(1);
                angle = Vector2.Angle(direction, Vector2.up);
                from = 0.0f;
                to = -90.0f;
            }
            else
            {
                entity.WeaponRoot.SetLocalScaleX(-1);
                angle = Vector2.Angle(direction, Vector2.left);
                from = 0.0f;
                to = -90.0f;
            }
            
            var weapon = entity.Weapon;
            weapon.transform.SetLocalPositionY(m_Amplitude);
            weapon.transform.SetLocalEulerRotateZ(from);
            weapon.gameObject.SetActive(true);
            var seq = DOTween.Sequence();
            seq.Append(TweenHelper.TweenDOLocalMoveY(weapon.transform, -m_Amplitude, m_Duration));
            seq.Join(TweenHelper.TweenDOLocalRotateZ(weapon.transform, to, m_Duration));

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