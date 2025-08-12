using UnityEngine;

namespace App.Common.Utilities.UtilityUnity.Runtime.Extensions
{
    public static class TransformExtensions
    {
        // position
        public static void SetPositionX(this Transform transform, float value)
        {
            var position = transform.position;
            position.x = value;
            transform.position = position;
        }
        
        public static void SetPositionY(this Transform transform, float value)
        {
            var position = transform.position;
            position.y = value;
            transform.position = position;
        }
        
        public static void SetPositionZ(this Transform transform, float value)
        {
            var position = transform.position;
            position.z = value;
            transform.position = position;
        }
        
        // local position
        
        public static void SetLocalPositionX(this Transform transform, float value)
        {
            var position = transform.localPosition;
            position.x = value;
            transform.localPosition = position;
        }
        
        public static void SetLocalPositionY(this Transform transform, float value)
        {
            var position = transform.localPosition;
            position.y = value;
            transform.localPosition = position;
        }
        
        public static void SetLocalPositionZ(this Transform transform, float value)
        {
            var position = transform.localPosition;
            position.z = value;
            transform.localPosition = position;
        }
        
        // rotate
        
        public static void SetEulerRotateX(this Transform transform, float value)
        {
            var angles = transform.eulerAngles;
            angles.x = value;
            transform.eulerAngles = angles;
        }
        
        public static void SetEulerRotateY(this Transform transform, float value)
        {
            var angles = transform.eulerAngles;
            angles.y = value;
            transform.eulerAngles = angles;
        }
        
        public static void SetEulerRotateZ(this Transform transform, float value)
        {
            var angles = transform.eulerAngles;
            angles.z = value;
            transform.eulerAngles = angles;
        }
        
        // local rotate
        
        public static void SetLocalEulerRotateZ(this Transform transform, float value)
        {
            var angles = transform.localEulerAngles;
            angles.z = value;
            transform.localEulerAngles = angles;
        }
        
        // local scale
        
        public static void SetLocalScaleX(this Transform transform, float value)
        {
            var angles = transform.localScale;
            angles.x = value;
            transform.localScale = angles;
        }
    }
}